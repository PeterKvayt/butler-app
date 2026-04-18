namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal sealed record UtilityServicesBillImageCommandArg : IPathCommandArg
{
    public string Path { get; }

    private UtilityServicesBillImageCommandArg(string path)
    {
        Path = path;
    }

    public static implicit operator UtilityServicesBillImageCommandArg(string path) => new(path);
    public static implicit operator string?(UtilityServicesBillImageCommandArg? arg) => arg?.Path;
}
