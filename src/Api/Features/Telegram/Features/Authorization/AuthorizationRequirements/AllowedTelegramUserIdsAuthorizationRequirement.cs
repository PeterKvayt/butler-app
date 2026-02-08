using Microsoft.AspNetCore.Authorization;

namespace Api.Features.Telegram.Features.Authorization.AuthorizationRequirements;

internal sealed class AllowedTelegramUserIdsAuthorizationRequirement(IReadOnlySet<string> userIds) : IAuthorizationRequirement
{
    internal IReadOnlySet<string> UserIds { get; } = userIds;
}
