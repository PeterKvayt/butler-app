using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Infrastructure.FileSystem.Abstractions;
using Api.Infrastructure.FileSystem.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.UploadFile;

internal sealed class UploadFileTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileSystemService _fileSystemService;

    private UploadFileTelegramCommandArgs _args = new();

    private string _nextArgMessage = string.Empty;

    public ITelegramCommandArgs Arguments
    {
        get => _args;
        set {
            if (value is not UploadFileTelegramCommandArgs args)
            {
                throw new InvalidOperationException($"Unsupported args of type {value.GetType()}");
            }

            _args = args;
        }
    }
    
    public UploadFileTelegramCommandArgsBuilder(ITelegramBotClient telegramBotClient, IFileSystemService fileSystemservice)
    {
        _telegramBotClient = telegramBotClient;
        _fileSystemService = fileSystemservice;
    }

    public async ValueTask AddAgrumentAsync(Message message)
    {
        if (!_args.ChatId.HasValue)
        {
            _args.ChatId = message.Chat.Id;
        }

        if (string.IsNullOrWhiteSpace(_args.TempFilePath))
        {
            var fileId = message.Photo?.LastOrDefault()?.FileId // TODO: add extension to get the highest quality photo
                ?? message.Document?.FileId; 
            if (string.IsNullOrWhiteSpace(fileId)) // if message does not have a photo
            {
                _nextArgMessage = "Upload the photo";
                return;
            }

            var fileInfo = await _telegramBotClient.GetFile(fileId);
            using var fileStream = new MemoryStream((int)fileInfo.FileSize);
            await _telegramBotClient.DownloadFile(fileInfo.FilePath, fileStream);
            var relativeFilePath = $"{message.Chat.Id}/{fileInfo.FileUniqueId}{Path.GetExtension(fileInfo.FilePath)}";
            await _fileSystemService.SaveFileAsync(fileStream, relativeFilePath);

            _args.TempFilePath = relativeFilePath;
        }
    }

    public Task RequestNextAgrumentAsync() => _telegramBotClient.SendMessage(_args.GetChatId(), _nextArgMessage);

    public bool IsArgumentsFilledIn()
    {
        return _args.ChatId.HasValue && !string.IsNullOrWhiteSpace(_args.TempFilePath);
    }
}
