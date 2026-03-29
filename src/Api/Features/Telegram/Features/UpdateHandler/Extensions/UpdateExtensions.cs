using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Extensions;

internal static class UpdateExtensions
{
    public static Message GetRequiredMessage(this Update update) => update.Message ?? throw new InvalidOperationException("Message is null");
}
