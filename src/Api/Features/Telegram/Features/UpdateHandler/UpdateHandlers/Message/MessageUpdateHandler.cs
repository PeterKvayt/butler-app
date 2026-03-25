using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Cancel.Extensions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommand;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;
using Api.Features.Telegram.Features.Command.Services.CommandContext;
using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using Api.Features.Telegram.Features.UpdateHandler.Extensions;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.UpdateHandler.UpdateHandlers.Message;

internal sealed class MessageUpdateHandler : ITelegramUpdateHandler
{
    private readonly ICommandContextService _commandContextService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITelegramCommandArgsBuilderProvider _builderProvider;
    private readonly ITelegramCommandProvider _telegramCommandProvider;
    private readonly ITelegramCommandInfoProvider _telegramCommandInfoProvider;

    public MessageUpdateHandler(
        ICommandContextService commandContextService,
        IHttpContextAccessor httpContextAccessor,
        ITelegramCommandArgsBuilderProvider builderProvider,
        ITelegramCommandProvider telegramCommandProvider,
        ITelegramCommandInfoProvider telegramCommandInfoProvider)
    {
        _commandContextService = commandContextService;
        _httpContextAccessor = httpContextAccessor;
        _builderProvider = builderProvider;
        _telegramCommandProvider = telegramCommandProvider;
        _telegramCommandInfoProvider = telegramCommandInfoProvider;
    }

    public bool IsSupported(Update update) => update.Type == UpdateType.Message;

    public async ValueTask HandleAsync(Update update)
    {
        var message = update.Message;

        if (message == null)
        {
            throw new InvalidOperationException("Message is null");
        }

        var userId = _httpContextAccessor.GetTelegramUserId();

        var commandContext = _commandContextService.GetContext(userId);

        var commandName = message.GetCommandName(_telegramCommandInfoProvider);

        ITelegramCommandArgsBuilder argumentsBuilder;

        if (commandContext == null)
        {
            if (commandName == null)
            {
                return;
            }

            argumentsBuilder = _builderProvider.GetBuilder(commandName);
            await argumentsBuilder.AddAgrumentAsync(message);
            commandContext = new CommandContextModel(argumentsBuilder.Arguments, commandName);

            _commandContextService.AddContext(userId, commandContext);
        }
        else
        {
            if (IsCancelCommand(commandName))
            {
                argumentsBuilder = _builderProvider.GetBuilder(commandName);
                await argumentsBuilder.AddAgrumentAsync(message);
                commandContext = new CommandContextModel(argumentsBuilder.Arguments, commandName);
            }
            else
            {
                argumentsBuilder = _builderProvider.GetBuilder(commandContext);
                await argumentsBuilder.AddAgrumentAsync(message);
                commandContext.CommandArgs = argumentsBuilder.Arguments;
            }
        }

        if (!argumentsBuilder.IsArgumentsFilledIn())
        {
            await argumentsBuilder.RequestNextAgrumentAsync();
            return;
        }

        var command = _telegramCommandProvider.GetCommand(commandContext.CommandName);
        await command.ExecuteAsync(commandContext.CommandArgs);

        await _commandContextService.RemoveContextAsync(userId);
    }

    private bool IsCancelCommand([NotNullWhen(true)] string? commandName)
    {
        if (commandName == null)
        {
            return false;
        }

        return _telegramCommandInfoProvider.CommandInfos.FirstOrDefault(e => e.IsCancelCommand()).Name == commandName;
    }
}
