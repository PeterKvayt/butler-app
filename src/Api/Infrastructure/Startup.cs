using Api.Features.Ping;
using Api.Features.Telegram;
using Api.Infrastructure.Logging;
using System.Text.Json;

namespace Api.Infrastructure;

internal static class Statup 
{
    internal static WebApplicationBuilder ConfigureApp(this WebApplicationBuilder builder)
    {
        builder
            .AddLogging()
            .AddTelegram();

        builder.Services
            .AddAuthentication();
        builder.Services
            .AddAuthorization();

        builder.Services
            .AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        return builder;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app
            .UsePing()
            .UseTelegram();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        return app;
    }
}