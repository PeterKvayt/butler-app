using Api.Infrastructure.Logging;
using Api.Features.Ping;
using Api.Infrastructure.Telegram;
using Api.Features.TelegramTest;

namespace Api.Infrastructure;

internal static class Statup 
{
    internal static WebApplicationBuilder ConfigureApp(this WebApplicationBuilder builder)
    {
        builder
            .AddLogging()
            .AddTelegramBot()
            .AddTelegramUpdateHandler();

        return builder;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();
        
        app
            .AddPingEndpoint()
            .AddTelegramUpdateEndpoint();

        return app;
    }
}