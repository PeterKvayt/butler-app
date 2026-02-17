using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authorization.Constants;
using Api.Features.Telegram.Features.UpdateHandler.EndpointFilters;
using Api.Features.Telegram.Features.UpdateHandler.Services.TelegramUpdate;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler;

internal static class Endpoint
{
    internal const string Segment = "bot/handle";

    internal static IEndpointRouteBuilder AddTelegramUpdateHandlerEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{Segment}", HandleAsync)
            .AddEndpointFilter<TelegramWebhookProtectionEndpointFilter>()
            .RequireAuthorization(AuthorizationPolicies.TelegramWebhook);

        return builder;
    }

    private static async Task HandleAsync(
        HttpContext context,
        ITelegramUpdateService service)
    {
        var update = (Update)context.Items[HttpContextItemKeys.TelegramIncomingUpdate];

        await service.ProcessUpdateAsync(update);
    }
}