namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal struct ElectricityCounterImageCommandArg
{
    public string Path;

    public static implicit operator ElectricityCounterImageCommandArg(string path) => new() { Path = path };
    public static implicit operator string(ElectricityCounterImageCommandArg arg) => arg.Path;
}
