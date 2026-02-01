using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Abstractions;

internal interface ITelegramUpdateHandler
{
    internal bool IsSupported(Update update);

    internal ValueTask HandleAsync(Update update);
}
