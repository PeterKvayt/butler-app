using Api.Features.Telegram.Features.Authentication.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Api.Features.Telegram.Features.Command.Services.CommandArgument;

internal sealed class CommandArgumentService : ICommandArgumentService
{
    private readonly IFusionCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommandArgumentService(IFusionCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    public T? Get<T>()
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        return Get<T>(userId);
    }

    public T GetRequired<T>()
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        return Get<T>(userId) ?? throw new InvalidOperationException($"Required argument of type {typeof(T).FullName} was null for user: {userId}");
    }

    private T? Get<T>(long userId)
    {
        return _cache.GetOrDefault<T>(GetKey<T>(userId), null);
    }

    public void Set<T>(T value)
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        _cache.Set(GetKey<T>(userId), value, options => options.SetDurationInfinite());
    }

    private static string GetKey<T>(long userId) => $"{userId}.TelegramCommandArgument.{typeof(T).FullName}";
}
