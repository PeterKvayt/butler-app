namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal sealed record HotWaterCounterImageCommandArg : IPathCommandArg
{
    public string Path { get; }

    private HotWaterCounterImageCommandArg(string path)
    {
        Path = path;
    }


    public static implicit operator HotWaterCounterImageCommandArg(string path) => new(path);
    public static implicit operator string?(HotWaterCounterImageCommandArg? arg) => arg?.Path;
}
