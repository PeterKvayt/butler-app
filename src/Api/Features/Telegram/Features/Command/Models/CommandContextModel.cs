using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Models;

internal sealed record CommandContextModel(ITelegramCommandArgs commandArgs, string commandName)
{
    internal string CommandName { get; } = commandName;
    internal ITelegramCommandArgs CommandArgs { get; set; } = commandArgs;
}
