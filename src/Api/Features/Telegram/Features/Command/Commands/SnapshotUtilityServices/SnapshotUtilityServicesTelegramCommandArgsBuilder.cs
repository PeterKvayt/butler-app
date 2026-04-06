using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ICommandArgumentService _commandArgumentService;
    private readonly IFileBufferService _fileBufferService;

    private string _nextArgMessage = string.Empty;

    private ChatTelegramCommandArg? _chatArg;
    private HotWaterCounterImageCommandArg? _hotWaterArg;
    private ColdWaterCounterImageCommandArg? _coldWaterArg;
    private CommunityServicesBillImageCommandArg? _communityServicesBillArg;
    private ElectricityCounterImageCommandArg? _electricityCounterArg;
    private UtilityServicesBillImageCommandArg? _utilityServicesBillArg;

    public SnapshotUtilityServicesTelegramCommandArgsBuilder(
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

        _coldWaterArg = _commandArgumentService.Get<ColdWaterCounterImageCommandArg>();
        if (_coldWaterArg == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload cold water counter photo";
                return;
            }

            _coldWaterArg = await GetBufferFilePathAsync(message);
            _commandArgumentService.Set(_coldWaterArg);
            _nextArgMessage = "Upload community services bill photo";
            return;
        }

        _communityServicesBillArg = _commandArgumentService.Get<CommunityServicesBillImageCommandArg>();
        if (_communityServicesBillArg == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload community services bill photo";
                return;
            }

            _communityServicesBillArg = await GetBufferFilePathAsync(message);
            _commandArgumentService.Set(_communityServicesBillArg);
            _nextArgMessage = "Upload electricity counter photo";
            return;
        }

        _electricityCounterArg = _commandArgumentService.Get<ElectricityCounterImageCommandArg>();
        if (_electricityCounterArg == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload electricity counter photo";
                return;
            }

            _electricityCounterArg = await GetBufferFilePathAsync(message);
            _commandArgumentService.Set(_electricityCounterArg);
            _nextArgMessage = "Upload hot water counter photo";
            return;
        }

        _hotWaterArg = _commandArgumentService.Get<HotWaterCounterImageCommandArg>();
        if (_hotWaterArg == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload hot water counter photo";
                return;
            }

            _hotWaterArg = await GetBufferFilePathAsync(message);
            _commandArgumentService.Set(_hotWaterArg);
            _nextArgMessage = "Upload utility services bill photo";
            return;
        }

        _utilityServicesBillArg = _commandArgumentService.Get<UtilityServicesBillImageCommandArg>();
        if (_utilityServicesBillArg == null)
        {
            if (string.IsNullOrWhiteSpace(message.GetFileId()))
            {
                _nextArgMessage = "Upload utility services bill photo";
                return;
            }

            _utilityServicesBillArg = await GetBufferFilePathAsync(message);
            _commandArgumentService.Set(_utilityServicesBillArg);
            return;
        }
    }

    public Task RequestNextAgrumentAsync() => _telegramBotClient.SendMessage(_chatArg.Value.Id, _nextArgMessage);

    public bool IsArgumentsFilledIn()
    {
        return _chatArg.HasValue 
            && _hotWaterArg.HasValue
            && _coldWaterArg.HasValue
            && _communityServicesBillArg.HasValue
            && _electricityCounterArg.HasValue
            && _utilityServicesBillArg.HasValue
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