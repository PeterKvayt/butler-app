using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Enums;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Commands.Constants;
using Api.Infrastructure.FileSystem.Abstractions;
using Microsoft.Extensions.ML;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly PredictionEnginePool<ImageData, ImagePrediction> _predictionEngine;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileBufferService _fileBufferService;

    public ClassifyImageTelegramCommand(
        TimeProvider timeProvider,
        ITelegramBotClient telegramBotClient,
        PredictionEnginePool<ImageData, ImagePrediction> predictionEngine,
        IWebHostEnvironment webHostEnvironment,
        IFileBufferService fileBufferService)
    {
        _telegramBotClient = telegramBotClient;
        _predictionEngine = predictionEngine;
        _webHostEnvironment = webHostEnvironment;
        _fileBufferService = fileBufferService;
    }

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.ClassifyImage,
        Description = "Classifies an image"
    };

    public async ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        if (commandArgs is not ClassifyImageTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{commandArgs.GetType().Name} is not supported");
        }

        var data = new ImageData
        {
            ImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "tg", "files", args.GetImagePath())
        };

        var prediction = _predictionEngine.Predict(data);

        await _telegramBotClient.SendMessage(args.GetChatId(), $"Image is {GetImgType(prediction.PredictedLabelValue)} ({prediction.PredictedLabelValue}). Scores: {string.Join(";", prediction.Score.OrderDescending().Select(x => x.ToString()))}.");
    }

    private string GetImgType(string predictedLabelValue)
    {
        if (!int.TryParse(predictedLabelValue, out var intCategory))
        {
            return "not classified";
        }

        var category = (ImageCategory)intCategory;

        if (!Enum.IsDefined(category))
        {
            return "not classified";
        }

        return category.ToString();
    }
}
