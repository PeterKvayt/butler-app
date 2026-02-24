using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Commands.Constants;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.UploadFile;

internal sealed class UploadFileTelegramCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileBufferService _fileBufferService;

    public UploadFileTelegramCommand(
        ITelegramBotClient telegramBotClient, 
        IFileSystemService fileSystemService,
        IFileBufferService fileBufferService)
    {
        _telegramBotClient = telegramBotClient;
        _fileSystemService = fileSystemService;
        _fileBufferService = fileBufferService;
    }

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

        var filePath = args.GetTempFilePath();

        var file = await _fileBufferService.GetFileAsync(filePath);

        await _fileSystemService.SaveFileAsync(file, $"/bot/{filePath}");

        await _fileBufferService.DeleteFileAsync(filePath);

        await _telegramBotClient.SendMessage(args.GetChatId(), "File uploaded to disk");
    }
}
