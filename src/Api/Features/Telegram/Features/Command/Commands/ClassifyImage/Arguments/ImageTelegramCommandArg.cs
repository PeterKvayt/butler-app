using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;

internal struct ImageTelegramCommandArg
{
    public string Path;

    public static implicit operator ImageTelegramCommandArg(string path) => new() { Path = path };
    public static implicit operator string(ImageTelegramCommandArg arg) => arg.Path;
}
