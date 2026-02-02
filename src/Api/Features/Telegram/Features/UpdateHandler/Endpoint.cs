using Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler;

internal static class Endpoint
{
    internal const string Segment = "bot/handle";

    internal static IEndpointRouteBuilder AddTelegramUpdateHandlerEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{Segment}", HandleAsync);

        return builder;
    }

    private static async Task HandleAsync(Update update, IUpdateHandlerService service)
    {
        await service.HandleAsync(update);
    }
}