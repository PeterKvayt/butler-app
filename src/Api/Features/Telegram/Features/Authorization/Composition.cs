using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authorization.Constants;

namespace Api.Features.Telegram.Features.Authorization;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.TelegramWebhook, policy =>
            {
                policy.AddAuthenticationSchemes(AuthenticationSchemas.TelegramWebhook);
                policy.RequireAuthenticatedUser();
            });

        return builder;
    }
}
