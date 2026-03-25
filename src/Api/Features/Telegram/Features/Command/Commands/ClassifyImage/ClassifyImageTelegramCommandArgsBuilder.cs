using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileBufferService _fileBufferService;

    private ClassifyImageTelegramCommandArgs _args = new();

    private string _nextArgMessage = string.Empty;

    public ITelegramCommandArgs Arguments
    {
        get => _args;
        set {
            if (value is not ClassifyImageTelegramCommandArgs args)
            {
                throw new InvalidOperationException($"Unsupported args of type {value.GetType()}");
            }

            _args = args;
        }
    }
    
    public ClassifyImageTelegramCommandArgsBuilder(ITelegramBotClient telegramBotClient, IFileBufferService fileSBufferService)
    {
        _telegramBotClient = telegramBotClient;
        _fileBufferService = fileSBufferService;
    }

    public async ValueTask AddAgrumentAsync(Message message)
    {
        if (!_args.ChatId.HasValue)
        {
            _args.ChatId = message.Chat.Id;
        }

        if (_args.ImagePath == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload photo";
                return;
            }

            _args.ImagePath = await GetBufferFilePathAsync(message);

            return;
        }
    }

    public Task RequestNextAgrumentAsync() => _telegramBotClient.SendMessage(_args.GetChatId(), _nextArgMessage);

    public bool IsArgumentsFilledIn()
    {
        return _args.ChatId.HasValue 
            && _args.ImagePath != null
            ;
    }

    private async Task<string> GetBufferFilePathAsync(Message message)
    {
        var fileId = message.GetFileId()
            ?? throw new InvalidOperationException("FileId is null");

        var fileInfo = await _telegramBotClient.GetFile(fileId);

        using var fileStream = new MemoryStream();
        var filePath = fileInfo.FilePath 
            ?? throw new InvalidOperationException($"File path is null for fileId {fileId} in chat {message.Chat.Id}");
        await _telegramBotClient.DownloadFile(filePath, fileStream);
        fileStream.Seek(0, SeekOrigin.Begin);

        var relativeFilePath = $"{message.Chat.Id}/{fileInfo.FileUniqueId}{Path.GetExtension(fileInfo.FilePath)}";
        await _fileBufferService.SaveFileAsync(fileStream, relativeFilePath);

        return relativeFilePath;
    } 
}