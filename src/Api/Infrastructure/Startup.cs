using Api.Infrastructure.Logging;
using Api.Features.Ping;

namespace Api.Infrastructure;

internal static class Statup 
{
    internal static WebApplicationBuilder ConfigureApp(this WebApplicationBuilder builder)
    {
        builder
            .AddLogging();

        return builder;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.AddPingEndpoint();

        return app;
    }
}