using Api.Features.Telegram.Features.Infrastructure.Extensions;
using Api.Features.Telegram.Features.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;

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
        await app.SetupTelegramCommandsAsync();

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
}