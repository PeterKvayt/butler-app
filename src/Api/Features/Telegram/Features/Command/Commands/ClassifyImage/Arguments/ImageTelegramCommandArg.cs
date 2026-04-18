using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;

internal sealed record ImageTelegramCommandArg : IPathCommandArg
{
    public string Path { get; }

    public ImageTelegramCommandArg(string path)
    {
        Path = path;
    }

    public static implicit operator ImageTelegramCommandArg(string path) => new(path);
    public static implicit operator string(ImageTelegramCommandArg arg) => arg.Path;
}
