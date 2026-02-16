using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommand;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;
using Api.Features.Telegram.Features.Command.Services.CommandContext;
using Api.Features.Telegram.Features.Commands.Constants;
using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.UpdateHandler.UpdateHandlers.Message;

internal sealed class MessageUpdateHandler(
    ICommandContextService commandContextService, 
    IHttpContextAccessor httpContextAccessor,
    ITelegramCommandArgsBuilderProvider builderProvider,
    ITelegramCommandProvider telegramCommandProvider)
    : ITelegramUpdateHandler
{
    private readonly ICommandContextService _commandContextService = commandContextService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ITelegramCommandArgsBuilderProvider _builderProvider = builderProvider;
    private readonly ITelegramCommandProvider _telegramCommandProvider = telegramCommandProvider;

    private readonly HashSet<string> _commandNames = [
        CommandNames.Cancel,
        CommandNames.NeedLove
    ];

    public bool IsSupported(Update update) => update.Type == UpdateType.Message;

    public async ValueTask HandleAsync(Update update)
    {
        if (update.Message == null)
        {
            throw new InvalidOperationException("Message is null");
        }

        var userId = _httpContextAccessor.GetTelegramUserId();

        var commandContext = _commandContextService.GetContext(userId);
        ITelegramCommandArgsBuilder argumentsBuilder;

        if (commandContext == null)
        {
            var commandName = ConvertToCommandName(update.Message.Text);
            
            var isCommand = _commandNames.Contains(commandName);
            if (!isCommand)
            {
                return;
            }

            argumentsBuilder = _builderProvider.GetBuilder(commandName);
            argumentsBuilder.AddAgrument(update.Message);
            commandContext = new CommandContextModel(argumentsBuilder.Arguments, commandName);
            _commandContextService.AddContext(userId, commandContext);
        }
        else
        {
            argumentsBuilder = _builderProvider.GetBuilder(commandContext.CommandName);
            argumentsBuilder.Arguments = commandContext.CommandArgs;
            argumentsBuilder.AddAgrument(update.Message);
            commandContext.CommandArgs = argumentsBuilder.Arguments;
        }

        if (!argumentsBuilder.IsArgumentsFilledIn())
        {
            await argumentsBuilder.RequestNextAgrumentAsync();
            return;
        }

        var command = _telegramCommandProvider.GetCommand(commandContext.CommandName);
        await command.ExecuteAsync(commandContext.CommandArgs);

        _commandContextService.RemoveContext(userId);
    }

    private static string? ConvertToCommandName(string? messageText)
    {
        return messageText?.TrimStart('/');
    }
}
