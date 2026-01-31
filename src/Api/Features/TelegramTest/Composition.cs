using Api.Features.TelegramTest.Services.UpdateHandler;

namespace Api.Features.TelegramTest;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramUpdateHandler(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUpdateHandlerService, UpdateHandlerService>();
        
        return builder;
    }
}