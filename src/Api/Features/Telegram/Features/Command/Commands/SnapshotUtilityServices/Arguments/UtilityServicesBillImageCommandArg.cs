namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal struct UtilityServicesBillImageCommandArg
{
    public string Path;

    public static implicit operator UtilityServicesBillImageCommandArg(string path) => new() { Path = path };
    public static implicit operator string(UtilityServicesBillImageCommandArg arg) => arg.Path;
}
