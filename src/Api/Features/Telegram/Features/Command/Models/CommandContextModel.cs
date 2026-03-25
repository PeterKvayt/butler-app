using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Models;

internal sealed record CommandContextModel
{
    public CommandContextModel(ITelegramCommandArgs commandArgs, string commandName)
    {
        CommandName = commandName;
        CommandArgs = commandArgs;
    }

    internal string CommandName { get; }
    internal ITelegramCommandArgs CommandArgs { get; set; }
}
