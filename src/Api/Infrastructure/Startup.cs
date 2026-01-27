using Api.Features.TelegramUpdate;
using Api.Infrastructure.Logging;
using Api.Infrastructure.TelegramBot;

namespace Api.Infrastructure;

internal static class Statup 
{
    internal static WebApplicationBuilder ConfigureApp(this WebApplicationBuilder builder)
    {
        builder
            .AddLogging()
            .AddTelegramBot()
            .AddUpdateHandler();

        return builder;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.AddTelegramUpdateEndpoint();

        return app;
    }
}