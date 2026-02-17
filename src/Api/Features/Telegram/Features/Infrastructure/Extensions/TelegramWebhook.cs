using Api.Features.Telegram.Features.Infrastructure.Options;
using Api.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Infrastructure.Extensions;

internal static class TelegramWebhook
{
    internal static async Task SetupTelegramWebhookAsync(this WebApplication app)
    {
        var telegramOptions = app.Services.GetRequiredService<IOptions<TelegramOptions>>().Value;
        var appOptions = app.Services.GetRequiredService<IOptions<AppOptions>>().Value;
        var webhookUrl = $"{appOptions.BaseUrl}/{UpdateHandler.Endpoint.Segment}";
        var client = app.Services.GetRequiredService<ITelegramBotClient>();

        await client.SetWebhook(webhookUrl,
            allowedUpdates: [UpdateType.Message],
            dropPendingUpdates: false,
            secretToken: telegramOptions.WebhookSecret
        );
    }
}
