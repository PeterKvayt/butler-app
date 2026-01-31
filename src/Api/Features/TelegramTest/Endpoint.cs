using Api.Features.TelegramTest.Services.UpdateHandler;
using Telegram.Bot.Types;

namespace Api.Features.TelegramTest;

internal static class Endpoint
{
    internal const string Segment = "update";

    internal static IEndpointRouteBuilder AddTelegramUpdateEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{Segment}", HandleAsync);

        return builder;
    }

    private static async Task HandleAsync(Update update, IUpdateHandlerService service)
    {
        await service.HandleAsync(update);
    }
}