using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Abstractions;

internal interface ITelegramCommand
{
    internal CommandInfo CommandInfo { get; }
    internal ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs);
}
