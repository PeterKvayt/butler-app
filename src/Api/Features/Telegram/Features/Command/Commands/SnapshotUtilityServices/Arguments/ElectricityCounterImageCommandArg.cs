namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal sealed record ElectricityCounterImageCommandArg : IPathCommandArg
{
    public string Path { get; }

    private ElectricityCounterImageCommandArg(string path)
    {
        Path = path;
    }

    public static implicit operator ElectricityCounterImageCommandArg(string path) => new(path);
    public static implicit operator string?(ElectricityCounterImageCommandArg? arg) => arg?.Path;
}
