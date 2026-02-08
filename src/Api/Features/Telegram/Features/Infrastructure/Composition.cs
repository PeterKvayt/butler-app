using Api.Features.Telegram.Features.Infrastructure.Extensions;
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
        await app.SetupTelegramCommandsAsync();

        return app;
    }

    private static WebApplicationBuilder AddTelegramBot(this WebApplicationBuilder builder)
    {
        var botToken = builder.Configuration.GetValue<string>("Telegram:BotToken")
            ?? throw new InvalidOperationException("Telegram:BotToken is null");

        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

        return builder;
    }
}