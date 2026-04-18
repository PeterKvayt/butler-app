using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Enums;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Features.Telegram.Features.Commands.Constants;
using Microsoft.Extensions.ML;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly PredictionEnginePool<ImageData, ImagePrediction> _predictionEngine;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICommandArgumentService _commandArgumentService;

    public ClassifyImageTelegramCommand(
        TimeProvider timeProvider,
        ITelegramBotClient telegramBotClient,
        PredictionEnginePool<ImageData, ImagePrediction> predictionEngine,
        IWebHostEnvironment webHostEnvironment,
        ICommandArgumentService commandArgumentService)
    {
        _telegramBotClient = telegramBotClient;
        _predictionEngine = predictionEngine;
        _webHostEnvironment = webHostEnvironment;
        _commandArgumentService = commandArgumentService;
    }

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.ClassifyImage,
        Description = "Classifies an image"
    };

    public async ValueTask ExecuteAsync()
    {
        var imageArg = _commandArgumentService.Get<ImageTelegramCommandArg>();
        if (imageArg == null)
        {
            return;
        }

        var data = new ImageData
        {
            ImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "tg", "files", imageArg.Path)
        };

        var prediction = _predictionEngine.Predict(data);

        var chatArg = _commandArgumentService.GetRequired<ChatTelegramCommandArg>();
        await _telegramBotClient.SendMessage(chatArg.Id, $"Image is {GetImgType(prediction.PredictedLabelValue)} ({prediction.PredictedLabelValue}). Scores: {string.Join(";", prediction.Score.OrderDescending().Select(x => x.ToString()))}.");
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
