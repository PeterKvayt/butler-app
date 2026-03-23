using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Services.CommandContext;
using Api.Features.Telegram.Features.Commands.Constants;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed class CancelTelegramCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ICommandContextService _commandContextService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CancelTelegramCommand(
        ITelegramBotClient telegramBotClient,
        ICommandContextService commandContextService,
        IHttpContextAccessor httpContextAccessor)
    {
        _telegramBotClient = telegramBotClient;
        _commandContextService = commandContextService;
        _httpContextAccessor = httpContextAccessor;
    }

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.Cancel,
        Description = "Cancel the current command"
    };

    public async ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        if (commandArgs is not CancelTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{commandArgs.GetType().Name} is not supported");
        }

        var userId = _httpContextAccessor.GetTelegramUserId();

        var context = _commandContextService.GetContext(userId);
        var commandName = context?.CommandName;
        var text = commandName is null
            ? "The current command has been canceled"
            : $"The \"{commandName}\" command has been canceled";

        await _commandContextService.RemoveContextAsync(userId);

        await _telegramBotClient.SendMessage(args.GetChatId(), text);
    }
}
