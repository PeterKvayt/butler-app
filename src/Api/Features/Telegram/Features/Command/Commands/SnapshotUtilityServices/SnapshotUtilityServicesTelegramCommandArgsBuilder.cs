using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Enums;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Infrastructure.FileSystem.Abstractions;
using Microsoft.Extensions.ML;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly PredictionEnginePool<ImageData, ImagePrediction> _predictionEngine;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICommandArgumentService _commandArgumentService;
    private readonly IFileBufferService _fileBufferService;

    private string _nextArgMessage = string.Empty;
    private bool _argsFilledIn = false;

    private ChatTelegramCommandArg? _chatArg;

    public SnapshotUtilityServicesTelegramCommandArgsBuilder(
        PredictionEnginePool<ImageData, ImagePrediction> predictionEngine,
        ITelegramBotClient telegramBotClient,
        IWebHostEnvironment webHostEnvironment,
        ICommandArgumentService commandArgumentService,
        IFileBufferService fileSBufferService)
    {
        _predictionEngine = predictionEngine;
        _telegramBotClient = telegramBotClient;
        _webHostEnvironment = webHostEnvironment;
        _commandArgumentService = commandArgumentService;
        _fileBufferService = fileSBufferService;
    }

    public async ValueTask AddAgrumentAsync(Message message)
    {
        FillChatArg(message);

        if ("ok".Equals(message.Text, StringComparison.OrdinalIgnoreCase))
        {
            _argsFilledIn = true;
            return;
        }

        var fileId = message.GetFileId();
        if (string.IsNullOrWhiteSpace(fileId))
        {
            _nextArgMessage = "Upload photos";
            return;
        }

        var filePath = await GetBufferFilePathAsync(fileId);
        var prediction = Predict(filePath);
        var category = GetCategory(prediction);

        switch (category)
        {
            case ImageCategory.HotWaterCounter:
                _commandArgumentService.Set<HotWaterCounterImageCommandArg>(filePath);
                break;
            case ImageCategory.ColdWaterCounter:
                _commandArgumentService.Set<ColdWaterCounterImageCommandArg>(filePath);
                break;
            case ImageCategory.ElectricityCounter:
                _commandArgumentService.Set<ElectricityCounterImageCommandArg>(filePath);
                break;
            case ImageCategory.UtilityServicesBill:
                _commandArgumentService.Set<UtilityServicesBillImageCommandArg>(filePath);
                break;
            case ImageCategory.CommunityServicesBill:
                _commandArgumentService.Set<CommunityServicesBillImageCommandArg>(filePath);
                break;
            default:
                throw new NotSupportedException($"{category} is not supported");
        }

        var accuracy = Math.Round(prediction.Score.OrderDescending().First() * 100, 4);
        await _telegramBotClient.SendPhoto(message.Chat.Id, message.GetRequiredFileId(), $"{category}: {accuracy}%");
    }

    public Task RequestNextAgrumentAsync()
    {
        if (string.IsNullOrEmpty(_nextArgMessage))
        {
            return Task.CompletedTask;
        }

        if (_chatArg == null)
        {
            throw new InvalidOperationException("Chat arg is not setted up");
        }

        return _telegramBotClient.SendMessage(_chatArg.Value.Id, _nextArgMessage);
    }

    public bool IsArgumentsFilledIn() => _argsFilledIn;

    private void FillChatArg(Message message)
    {
        _chatArg = _commandArgumentService.Get<ChatTelegramCommandArg>();
        if (_chatArg == null)
        {
            _chatArg = message.Chat.Id;
            _commandArgumentService.Set(_chatArg);
        }
    }

    private async Task<string> GetBufferFilePathAsync(string fileId)
    {
        if (_chatArg == null)
        {
            throw new InvalidOperationException("Chat arg is not setted up");
        }

        var fileInfo = await _telegramBotClient.GetFile(fileId);

        using var fileStream = new MemoryStream();
        var filePath = fileInfo.FilePath
            ?? throw new InvalidOperationException($"File path is null for fileId {fileId} in chat {_chatArg.Value.Id}");
        await _telegramBotClient.DownloadFile(filePath, fileStream);
        fileStream.Seek(0, SeekOrigin.Begin);

        var relativeFilePath = $"{_chatArg.Value.Id}/{fileInfo.FileUniqueId}{Path.GetExtension(fileInfo.FilePath)}";
        await _fileBufferService.SaveFileAsync(fileStream, relativeFilePath);

        return relativeFilePath;
    }

    private ImagePrediction Predict(string filePath)
    {
        var data = new ImageData
        {
            ImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "tg", "files", filePath)
        };

        var prediction = _predictionEngine.Predict(data);

        return prediction;
    }

    private ImageCategory GetCategory(ImagePrediction prediction)
    {
        if (!int.TryParse(prediction.PredictedLabelValue, out var intCategory))
        {
            throw new InvalidOperationException($"Unable to parse PredictedLabelValue: {prediction.PredictedLabelValue}");
        }

        var category = (ImageCategory)intCategory;

        if (!Enum.IsDefined(category))
        {
            throw new InvalidOperationException($"Unable to parse PredictedLabelValue: {prediction.PredictedLabelValue}");
        }

        return category;
    }
}