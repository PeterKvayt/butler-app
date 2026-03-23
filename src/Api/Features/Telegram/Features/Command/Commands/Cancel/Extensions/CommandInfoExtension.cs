using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Commands.Constants;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel.Extensions;

internal static class CommandInfoExtension
{
    public static bool IsCancelCommand(this CommandInfo commandInfo) => commandInfo.Name == CommandNames.Cancel;
}
