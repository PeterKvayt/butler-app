using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Abstractions;

internal interface ITelegramCommandArgsDestroyer
{
    internal ValueTask DestroyAsync(ITelegramCommandArgs arguments);
}
