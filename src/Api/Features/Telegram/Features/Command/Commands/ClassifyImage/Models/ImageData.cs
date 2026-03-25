using Microsoft.ML.Data;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;

public class ImageData
{
    [LoadColumn(0)]
    public string ImagePath;

    [LoadColumn(1)]
    public string Label;
}
