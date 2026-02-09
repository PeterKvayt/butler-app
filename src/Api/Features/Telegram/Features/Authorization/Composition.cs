using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authorization.AuthorizationHandlers;
using Api.Features.Telegram.Features.Authorization.AuthorizationRequirements;
using Api.Features.Telegram.Features.Authorization.Constants;
using Api.Features.Telegram.Features.Infrastructure.Options;
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
                policy.Requirements.Add(CreateRequirement(builder.Configuration));
            });

        return builder;
    }

    private static AllowedTelegramUserIdsAuthorizationRequirement CreateRequirement(IConfiguration configuration)
    {
        var telegramOptions = configuration.GetSection("Telegram").Get<TelegramOptions>()
            ?? throw new InvalidOperationException($"{nameof(TelegramOptions)} is null");

        return new AllowedTelegramUserIdsAuthorizationRequirement(telegramOptions.AllowedUserIds);
    }
}
