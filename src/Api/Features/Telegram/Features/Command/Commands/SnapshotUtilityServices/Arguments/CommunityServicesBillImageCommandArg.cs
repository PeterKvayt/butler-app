namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal struct CommunityServicesBillImageCommandArg
{
    public string Path;

    public static implicit operator CommunityServicesBillImageCommandArg(string path) => new() { Path = path };
    public static implicit operator string(CommunityServicesBillImageCommandArg arg) => arg.Path;
}
