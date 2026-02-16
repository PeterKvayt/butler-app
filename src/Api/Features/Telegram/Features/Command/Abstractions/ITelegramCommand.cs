namespace Api.Features.Telegram.Features.Command.Abstractions;

internal interface ITelegramCommand
{
    internal ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs);
}
