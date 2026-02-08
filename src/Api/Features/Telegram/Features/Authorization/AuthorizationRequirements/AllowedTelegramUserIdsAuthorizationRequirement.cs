using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Features.Telegram.Features.Authorization.AuthorizationRequirements;

internal sealed class AllowedTelegramUserIdsAuthorizationRequirement(IReadOnlySet<string> userIds) : IAuthorizationRequirement
{
    private readonly IReadOnlySet<string> _allowedUserIds = userIds;

    internal bool IsAllowedUser(Claim userIdClaim) => _allowedUserIds.Contains(userIdClaim.Value);
}
