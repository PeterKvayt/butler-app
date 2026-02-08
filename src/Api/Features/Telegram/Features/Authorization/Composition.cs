using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authorization.AuthorizationHandlers;
using Api.Features.Telegram.Features.Authorization.AuthorizationRequirements;
using Api.Features.Telegram.Features.Authorization.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.Telegram.Features.Authorization;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IAuthorizationHandler, TelegramUserIdAuthorizationHandler>();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.TelegramWebhook, policy =>
            {
                policy.AddAuthenticationSchemes(AuthenticationSchemes.TelegramWebhook);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(CreateAllowedTelegramUserIdsAuthorizationRequirement(builder.Configuration));
            });

        return builder;
    }

    private static AllowedTelegramUserIdsAuthorizationRequirement CreateAllowedTelegramUserIdsAuthorizationRequirement(IConfiguration configuration)
    {
        var allowedUserIds = configuration.GetSection("Telegram:AllowedUserIds").Get<string[]>()
            ?? throw new InvalidOperationException("Telegram:AllowedUserIds is null");

        return new AllowedTelegramUserIdsAuthorizationRequirement(new HashSet<string>(allowedUserIds));
    }
}
