using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Models;

internal sealed record CommandContextModel(ITelegramCommandArgs commandArgs)
{
    internal ITelegramCommandArgs CommandArgs { get; init; } = commandArgs;
}
