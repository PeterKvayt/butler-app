using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;

internal struct HotWaterCounterImageCommandArg
{
    public string Path;

    public static implicit operator HotWaterCounterImageCommandArg(string path) => new() { Path = path };
    public static implicit operator string(HotWaterCounterImageCommandArg arg) => arg.Path;
}
