using Telegram.Bot;

namespace Api.Infrastructure.TelegramBot;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramBot(this WebApplicationBuilder builder)
    {
        var key = builder.Configuration.GetConnectionString("Telegram") 
            ?? throw new InvalidOperationException("Telegram key was null"); 
        var bot = new TelegramBotClient(key);

        builder.Services.AddSingleton<ITelegramBotClient>(bot);
        
        return builder;
    }

    internal static async Task ConfigureTelegramWebhookAsync(this WebApplication app)
    {
        var baseUrl = app.Configuration.GetValue<string>("App:BaseUrl")
            ?? throw new InvalidOperationException("App:BaseUrl is null");
        var webhookUrl = $"{baseUrl}/{Features.TelegramUpdate.Endpoint.Path}";

        var client = app.Services.GetRequiredService<ITelegramBotClient>();
        
        // TODO: separate to endpoints by Telegram.Bot.Types.Enums.UpdateType
        await client.SetWebhook(webhookUrl, allowedUpdates: []);
    }
}