using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Cancel.Extensions;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommand;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;
using Api.Features.Telegram.Features.Command.Services.CurrentCommand;
using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using Api.Features.Telegram.Features.UpdateHandler.Extensions;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.UpdateHandler.UpdateHandlers.Message;

internal sealed class MessageUpdateHandler : ITelegramUpdateHandler
{
    private readonly ICurrentCommandService _currentCommandService;
    private readonly ITelegramCommandArgsBuilderProvider _builderProvider;
    private readonly ITelegramCommandProvider _telegramCommandProvider;
    private readonly ITelegramCommandInfoProvider _telegramCommandInfoProvider;

    public MessageUpdateHandler(
        ICurrentCommandService currentCommandService,
        IHttpContextAccessor httpContextAccessor,
        ITelegramCommandArgsBuilderProvider builderProvider,
        ITelegramCommandProvider telegramCommandProvider,
        ITelegramCommandInfoProvider telegramCommandInfoProvider)
    {
        _currentCommandService = currentCommandService;
        _builderProvider = builderProvider;
        _telegramCommandProvider = telegramCommandProvider;
        _telegramCommandInfoProvider = telegramCommandInfoProvider;
    }

    public bool IsSupported(Update update) => update.Type == UpdateType.Message;

    public async ValueTask HandleAsync(Update update)
    {
        var message = update.GetRequiredMessage();
        var commandName = message.GetCommandName(_telegramCommandInfoProvider);

        ITelegramCommandArgsBuilder argumentsBuilder;

        if (IsCancelCommand(commandName))
        {
            argumentsBuilder = _builderProvider.GetBuilder(commandName);
        }
        else
        {
            commandName = _currentCommandService.GetOrCreate(commandName);

            if (commandName == null)
            {
                return;
            }

            argumentsBuilder = _builderProvider.GetBuilder(commandName);
        }
        
        await argumentsBuilder.AddAgrumentAsync(message);

        if (!argumentsBuilder.IsArgumentsFilledIn())
        {
            await argumentsBuilder.RequestNextAgrumentAsync();
            return;
        }

        var command = _telegramCommandProvider.GetCommand(commandName);
        await command.ExecuteAsync();

        _currentCommandService.Remove();
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
