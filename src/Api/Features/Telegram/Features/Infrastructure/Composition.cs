using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Infrastructure;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddTelegramBot();
        
        return builder;
    }

    internal static async Task<WebApplication> SetupTelegramInfrastructureAsync(this WebApplication app)
    {
        await app.SetupTelegramWebhookAsync();

        return app;
    }

    private static WebApplicationBuilder AddTelegramBot(this WebApplicationBuilder builder)
    {
        var botToken = builder.Configuration.GetValue<string>("Telegram:BotToken")
            ?? throw new InvalidOperationException("Telegram:BotToken is null");

        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

        return builder;
    }

    private static async Task SetupTelegramWebhookAsync(this WebApplication app)
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