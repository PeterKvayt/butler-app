using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileBufferService _fileBufferService;

    private SnapshotUtilityServicesTelegramCommandArgs _args = new();

    private string _nextArgMessage = string.Empty;

    public ITelegramCommandArgs Arguments
    {
        get => _args;
        set {
            if (value is not SnapshotUtilityServicesTelegramCommandArgs args)
            {
                throw new InvalidOperationException($"Unsupported args of type {value.GetType()}");
            }

            _args = args;
        }
    }
    
    public SnapshotUtilityServicesTelegramCommandArgsBuilder(ITelegramBotClient telegramBotClient, IFileBufferService fileSBufferService)
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

        if (_args.ColdWaterCounterFilePath == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload cold water counter photo";
                return;
            }

            _args.ColdWaterCounterFilePath = await GetBufferFilePathAsync(message);

            _nextArgMessage = "Upload community services bill photo";

            return;
        }

        if (_args.CommunityServicesBillFilePath == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload community services bill photo";
                return;
            }

            _args.CommunityServicesBillFilePath = await GetBufferFilePathAsync(message);

            _nextArgMessage = "Upload electricity counter photo";

            return;
        }

        if (_args.ElectricityCounterFilePath == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload electricity counter photo";
                return;
            }

            _args.ElectricityCounterFilePath = await GetBufferFilePathAsync(message);

            _nextArgMessage = "Upload hot water counter photo";

            return;
        }

        if (_args.HotWaterCounterFilePath == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload hot water counter photo";
                return;
            }

            _args.HotWaterCounterFilePath = await GetBufferFilePathAsync(message);

            _nextArgMessage = "Upload utility services bill photo";

            return;
        }

        if (_args.UtilityServicesBillFilePath == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload utility services bill photo";
                return;
            }

            _args.UtilityServicesBillFilePath = await GetBufferFilePathAsync(message);

            return;
        }
    }

    public Task RequestNextAgrumentAsync() => _telegramBotClient.SendMessage(_args.GetChatId(), _nextArgMessage);

    public bool IsArgumentsFilledIn()
    {
        return _args.ChatId.HasValue 
            && _args.ColdWaterCounterFilePath != null
            && _args.CommunityServicesBillFilePath != null
            && _args.ElectricityCounterFilePath != null
            && _args.HotWaterCounterFilePath != null
            && _args.UtilityServicesBillFilePath != null
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