using Api.Features.Telegram.Features.Authentication.Extensions;
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

        var commandName = ConvertToCommandName(update.Message.Text);

        if (string.IsNullOrWhiteSpace(commandName))
        {
            return;
        }

        var isCommand = _commandNames.Contains(commandName);

        if (!isCommand)
        {
            return;
        }

        await ProcessCommandAsync(commandName);
    }

    private async Task ProcessCommandAsync(string commandName)
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        var argumentsBuilder = _builderProvider.GetBuilder(commandName);
        var currentContext = _commandContextService.GetOrAddContext(userId, (userId) => new CommandContextModel(argumentsBuilder.Arguments));

        argumentsBuilder.Arguments = currentContext.CommandArgs;

        if (!argumentsBuilder.IsArgumentsFilledIn())
        {
            await argumentsBuilder.RequestNextAgrumentAsync();
            return;
        }

        var command = _telegramCommandProvider.GetCommand(commandName);
        await command.ExecuteAsync(argumentsBuilder.Arguments);

        _commandContextService.RemoveContext(userId);
    }

    private static string? ConvertToCommandName(string? messageText)
    {
        return messageText?.TrimStart('/');
    }
}
