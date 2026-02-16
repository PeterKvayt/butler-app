using Api.Features.Telegram.Features.Authentication.Constants;

namespace Api.Features.Telegram.Features.Authentication.Extensions;

internal static class IHttpContextAccessorExtensions
{
    internal static long GetTelegramUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is null");

        var userIdClaim = httpContext.User.Claims.First(e => e.Type.Equals(TelegramClaims.Id, StringComparison.Ordinal));
        var id = long.Parse(userIdClaim.Value);

        return id;
    }
}
