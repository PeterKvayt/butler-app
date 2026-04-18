using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Cancel.Arguments;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Features.Telegram.Features.Command.Services.CurrentCommand;
using Api.Features.Telegram.Features.Commands.Constants;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed class CancelTelegramCommand : ITelegramCommand
{
    private readonly ITelegramCommandArgsDestroyerProvider _telegramCommandArgsDestroyerProvider;
    private readonly ICurrentCommandService _currentCommandService;
    private readonly ICommandArgumentService _commandArgumentService;
    private readonly ITelegramBotClient _telegramBotClient;

    public CancelTelegramCommand(
        ITelegramCommandArgsDestroyerProvider telegramCommandArgsDestroyerProvider,
        ICurrentCommandService currentCommandService,
        ICommandArgumentService commandArgumentService,
        ITelegramBotClient telegramBotClient)
    {
        _telegramCommandArgsDestroyerProvider = telegramCommandArgsDestroyerProvider;
        _currentCommandService = currentCommandService;
        _commandArgumentService = commandArgumentService;
        _telegramBotClient = telegramBotClient;
    }

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.Cancel,
        Description = "Cancel the current command"
    };

    public async ValueTask ExecuteAsync()
    {
        var commandName = _currentCommandService.GetOrCreate(null);
        var chat = _commandArgumentService.GetRequired<ChatTelegramCommandArg>();

        if (commandName == null)
        {
            await _telegramBotClient.SendMessage(chat.Id, "There is no current command");
            return;
        }

        var argumentsDestroyer = _telegramCommandArgsDestroyerProvider.GetDestroyer(commandName);
        if (argumentsDestroyer != null)
        {
            await argumentsDestroyer.DestroyAsync();
        }

        await _telegramBotClient.SendMessage(chat.Id, $"The \"{commandName}\" command has been canceled");
    }
}
