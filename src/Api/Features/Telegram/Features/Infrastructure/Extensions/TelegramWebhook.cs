using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Infrastructure.Extensions;

internal static class TelegramWebhook
{
    internal static async Task SetupTelegramWebhookAsync(this WebApplication app)
    {
        var webhookSecret = app.Configuration.GetValue<string>("Telegram:WebhookSecret")
            ?? throw new InvalidOperationException("Telegram:WebhookSecret is null");
        var baseUrl = app.Configuration.GetValue<string>("App:BaseUrl")
            ?? throw new InvalidOperationException("App:BaseUrl is null");
        var webhookUrl = $"{baseUrl}/{UpdateHandler.Endpoint.Segment}";

        var client = app.Services.GetRequiredService<ITelegramBotClient>();

        await client.SetWebhook(webhookUrl,
            allowedUpdates: [UpdateType.Message],
            dropPendingUpdates: false,
            secretToken: webhookSecret
        );
    }
}
