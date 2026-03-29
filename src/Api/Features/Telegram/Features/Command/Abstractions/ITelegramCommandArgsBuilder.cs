using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Abstractions;

internal interface ITelegramCommandArgsBuilder
{
    internal ValueTask AddAgrumentAsync(Message message);
    internal Task RequestNextAgrumentAsync();
    internal bool IsArgumentsFilledIn();
}
