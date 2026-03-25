using Microsoft.ML.Data;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;

public class ImagePrediction : ImageData
{
    public float[] Score;

    [ColumnName("PredictedLabelValue")]
    public string PredictedLabelValue;
}
