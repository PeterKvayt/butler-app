using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Enums;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Infrastructure.FileSystem.Abstractions;
using Microsoft.Extensions.ML;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly PredictionEnginePool<ImageData, ImagePrediction> _predictionEngine;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ICommandArgumentService _commandArgumentService;
    private readonly IFileBufferService _fileBufferService;

    private string? _nextArgMessage;
    private bool _argsFilledIn = false;

    public ClassifyImageTelegramCommandArgsBuilder(
        PredictionEnginePool<ImageData, ImagePrediction> predictionEngine,
        IWebHostEnvironment webHostEnvironment,
        ITelegramBotClient telegramBotClient,
        ICommandArgumentService commandArgumentService,
        IFileBufferService fileSBufferService)
    {
        _predictionEngine = predictionEngine;
        _webHostEnvironment = webHostEnvironment;
        _telegramBotClient = telegramBotClient;
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

        if (string.IsNullOrWhiteSpace(message.GetFileId()))
        {
            _nextArgMessage = "Upload photo";
            return;
        }

        var filePath = await GetBufferFilePathAsync(message);
        var prediction = Predict(filePath);
        var category = GetCategory(prediction);

        if (category == ImageCategory.ElectricityCounter)
        {
            var oldImgArg = _commandArgumentService.Get<ImageTelegramCommandArg>();
            _commandArgumentService.Set<ImageTelegramCommandArg>(filePath);
            var accuracy = Math.Round(prediction.Score.OrderDescending().First() * 100, 4);
            await _telegramBotClient.SendPhoto(message.Chat.Id, message.GetRequiredFileId(), $"Accuracy: {accuracy}%");
            if (oldImgArg != null)
            {
                await _fileBufferService.DeleteFileAsync(oldImgArg);
            }
        }
        else
        {
            await _fileBufferService.DeleteFileAsync(filePath);
            _nextArgMessage = "Electricity counter photo expected";
        }
    }

    private void FillChatArg(Message message)
    {
        if (_commandArgumentService.Get<ChatTelegramCommandArg>() == null)
        {
            _commandArgumentService.Set<ChatTelegramCommandArg>(message.Chat.Id);
        }
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

    private ImagePrediction Predict(string filePath)
    {
        var data = new ImageData
        {
            ImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "tg", "files", filePath)
        };

        var prediction = _predictionEngine.Predict(data);

        return prediction;
    }

    public Task RequestNextAgrumentAsync()
    {
        if (_nextArgMessage == null)
        {
            return Task.CompletedTask;
        }

        var chatArg = _commandArgumentService.GetRequired<ChatTelegramCommandArg>();
        return _telegramBotClient.SendMessage(chatArg.Id, _nextArgMessage);
    }

    public bool IsArgumentsFilledIn()
    {
        return _argsFilledIn;
    }

    private async Task<string> GetBufferFilePathAsync(Message message)
    {
        var fileId = message.GetRequiredFileId();

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