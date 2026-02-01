using Api.Features.Telegram.Features.MessageProcess.Services.MessageHandle;
using Api.Features.Telegram.Features.UpdateHandler.Abstractions;

namespace Api.Features.Telegram.Features.MessageProcess;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramMessageProcess(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITelegramUpdateHandler, MessageHandlerService>();

        return builder;
    }
}
