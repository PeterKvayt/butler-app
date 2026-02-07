using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authorization.Constants;
using Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler;

internal static class Endpoint
{
    internal const string Segment = "bot/handle";

    internal static IEndpointRouteBuilder AddTelegramUpdateHandlerEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{Segment}", HandleAsync)
            .RequireAuthorization(AuthorizationPolicies.TelegramWebhook);

        return builder;
    }

    private static async Task HandleAsync(
        HttpContext context,
        IUpdateHandlerService service)
    {
        var update = (Update)context.Items[HttpContextItemKeys.TelegramIncomingUpdate];

        await service.HandleAsync(update);
    }
}