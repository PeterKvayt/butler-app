using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Commands.Constants;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.UploadFile;

internal sealed class UploadFileTelegramCommand(ITelegramBotClient telegramBotClient) : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.UploadFile,
        Description = "Command to upload a file"
    };

    public async ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        if (commandArgs is not UploadFileTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{commandArgs.GetType().Name} is not supported");
        }

        await _telegramBotClient.SendMessage(args.GetChatId(), "File uploaded");
    }
}
