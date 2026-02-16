using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using Api.Features.Telegram.Features.UpdateHandler.Services.TelegramUpdate;
using Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;
using Api.Features.Telegram.Features.UpdateHandler.UpdateHandlers.Message;

namespace Api.Features.Telegram.Features.UpdateHandler;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramUpdateProcess(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ITelegramUpdateService, TelegramUpdateService>()
            .AddScoped<ITelegramUpdateHandler, MessageUpdateHandler>();

        return builder;
    }

    internal static WebApplication UseTelegramUpdateProcess(this WebApplication app)
    {
        app.AddTelegramUpdateHandlerEndpoint();

        return app;
    }
}
