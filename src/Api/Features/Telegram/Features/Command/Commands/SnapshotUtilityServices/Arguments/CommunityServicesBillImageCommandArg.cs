namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal sealed record CommunityServicesBillImageCommandArg : IPathCommandArg
{
    public string Path { get; }

    private CommunityServicesBillImageCommandArg(string path)
    {
        Path = path;
    }

    public static implicit operator CommunityServicesBillImageCommandArg(string path) => new(path);
    public static implicit operator string?(CommunityServicesBillImageCommandArg? arg) => arg?.Path;
}
