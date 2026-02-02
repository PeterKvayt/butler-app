using Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;

namespace Api.Features.Telegram.Features.UpdateHandler;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramUpdateProcess(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUpdateHandlerService, UpdateHandlerService>();

        return builder;
    }

    internal static WebApplication UseTelegramUpdateProcess(this WebApplication app)
    {
        app.AddTelegramUpdateHandlerEndpoint();

        return app;
    }
}
