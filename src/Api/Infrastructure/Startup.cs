using Api.Features.Ping;
using Api.Features.Telegram;
using Api.Infrastructure.Logging;

namespace Api.Infrastructure;

internal static class Statup 
{
    internal static WebApplicationBuilder ConfigureApp(this WebApplicationBuilder builder)
    {
        builder
            .AddLogging()
            .AddTelegram();

        return builder;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app
            .UsePing()
            .UseTelegram();

        return app;
    }
}