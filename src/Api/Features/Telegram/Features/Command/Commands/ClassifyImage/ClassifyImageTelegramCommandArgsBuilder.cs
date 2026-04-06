using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ICommandArgumentService _commandArgumentService;
    private readonly IFileBufferService _fileBufferService;

    private ChatTelegramCommandArg? _chatArg;
    private ImageTelegramCommandArg? _imageArg;

    private string _nextArgMessage = string.Empty;

    public ClassifyImageTelegramCommandArgsBuilder(
        ITelegramBotClient telegramBotClient,
        ICommandArgumentService commandArgumentService,
        IFileBufferService fileSBufferService)
    {
        _telegramBotClient = telegramBotClient;
        _commandArgumentService = commandArgumentService;
        _fileBufferService = fileSBufferService;
    }

    public async ValueTask AddAgrumentAsync(Message message)
    {
        _chatArg = _commandArgumentService.Get<ChatTelegramCommandArg>();
        if (_chatArg == null)
        {
            _chatArg = message.Chat.Id;
            _commandArgumentService.Set(_chatArg);
        }

        _imageArg = _commandArgumentService.Get<ImageTelegramCommandArg>();
        if (_imageArg == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload photo";
                return;
            }

            _imageArg = await GetBufferFilePathAsync(message);
            _commandArgumentService.Set(_imageArg);
        }
    }

    public Task RequestNextAgrumentAsync() => _telegramBotClient.SendMessage(_chatArg.Value.Id, _nextArgMessage);

    public bool IsArgumentsFilledIn()
    {
        return _chatArg.HasValue 
            && _imageArg.HasValue;
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