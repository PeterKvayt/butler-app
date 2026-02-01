using Telegram.Bot;

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
        var key = builder.Configuration.GetConnectionString("Telegram")
            ?? throw new InvalidOperationException("Telegram key was null");
        var bot = new TelegramBotClient(key);

        builder.Services.AddSingleton<ITelegramBotClient>(bot);

        return builder;
    }

    private static async Task SetupTelegramWebhookAsync(this WebApplication app)
    {
        var baseUrl = app.Configuration.GetValue<string>("App:BaseUrl")
            ?? throw new InvalidOperationException("App:BaseUrl is null");
        var webhookUrl = $"{baseUrl}/{UpdateHandler.Endpoint.Segment}";

        var client = app.Services.GetRequiredService<ITelegramBotClient>();

        // TODO: separate to endpoints by Telegram.Bot.Types.Enums.UpdateType
        await client.SetWebhook(webhookUrl, allowedUpdates: []);
    }
}