using Api.Features.TelegramUpdate.Services.UpdateHandler;

namespace Api.Features.TelegramUpdate;

internal static class Composition
{
    internal static WebApplicationBuilder AddUpdateHandler(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUpdateHandlerService, UpdateHandlerService>();
        
        return builder;
    }
}