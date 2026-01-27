using Api.Features.TelegramUpdate.Services.UpdateHandler;
using Telegram.Bot.Types;

namespace Api.Features.TelegramUpdate;

internal static class Endpoint
{
    internal const string Path = "update";

    internal static void AddTelegramUpdateEndpoint(this IEndpointRouteBuilder builder) 
        => builder.MapPost($"/{Path}", HandleAsync);

    private static async Task HandleAsync(Update update, IUpdateHandlerService service)
    {
        await service.HandleAsync(update);
    }
}