using Api.Features.Telegram.Features.Infrastructure.Options;
using Api.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Infrastructure;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection("Telegram"));

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
        builder.Services.AddSingleton<ITelegramBotClient>(factory =>
        {
            var options = factory.GetRequiredService<IOptions<TelegramOptions>>().Value;
            return new TelegramBotClient(options.BotToken);
        });

        return builder;
    }

    private static async Task SetupTelegramWebhookAsync(this WebApplication app)
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