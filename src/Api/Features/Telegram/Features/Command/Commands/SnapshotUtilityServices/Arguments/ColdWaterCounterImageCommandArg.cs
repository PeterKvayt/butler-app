namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal sealed record ColdWaterCounterImageCommandArg : IPathCommandArg
{
    public string Path { get; }

    private ColdWaterCounterImageCommandArg(string path)
    {
        Path = path;
    }

    public static implicit operator ColdWaterCounterImageCommandArg(string path) => new(path);
    public static implicit operator string?(ColdWaterCounterImageCommandArg? arg) => arg?.Path;
}
