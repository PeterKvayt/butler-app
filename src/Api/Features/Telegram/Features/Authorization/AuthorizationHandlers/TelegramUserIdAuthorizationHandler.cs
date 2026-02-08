using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authorization.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.Telegram.Features.Authorization.AuthorizationHandlers;

internal sealed class TelegramUserIdAuthorizationHandler : AuthorizationHandler<AllowedTelegramUserIdsAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedTelegramUserIdsAuthorizationRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(e => e.Type.Equals(TelegramClaims.Id, StringComparison.Ordinal));

        if (userIdClaim == null)
        {
            return Task.CompletedTask;
        }

        if (requirement.UserIds.Contains(userIdClaim.Value))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
