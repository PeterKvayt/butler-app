namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal struct ColdWaterCounterImageCommandArg
{
    public string Path;

    public static implicit operator ColdWaterCounterImageCommandArg(string path) => new() { Path = path };
    public static implicit operator string(ColdWaterCounterImageCommandArg arg) => arg.Path;
}
