using Api.Features.Telegram.Features.Infrastructure;
using Api.Features.Telegram.Features.MessageProcess;
using Api.Features.Telegram.Features.UpdateHandler;

namespace Api.Features.Telegram;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegram(this WebApplicationBuilder builder)
    {
        builder
            .AddTelegramInfrastructure()
            .AddTelegramMessageProcess()
            .AddTelegramUpdateProcess();
        
        return builder;
    }

    internal static WebApplication UseTelegram(this WebApplication app)
    {
        app.UseTelegramUpdateProcess();

        return app;
    }

    internal static async Task<WebApplication> SetupTelegramAsync(this WebApplication app)
    {
        await app.SetupTelegramInfrastructureAsync();

        return app;
    }
}