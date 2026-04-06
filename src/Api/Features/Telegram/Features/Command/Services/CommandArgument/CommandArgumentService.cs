using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;
using ZiggyCreatures.Caching.Fusion;

namespace Api.Features.Telegram.Features.Command.Services.CommandArgument;

internal sealed class CommandArgumentService : ICommandArgumentService
{
    private readonly IFusionCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITelegramCommandArgsDestroyerProvider _telegramCommandArgsDestroyerProvider;

    public CommandArgumentService(
        IFusionCache cache, 
        IHttpContextAccessor httpContextAccessor,
        ITelegramCommandArgsDestroyerProvider telegramCommandArgsDestroyerProvider)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _telegramCommandArgsDestroyerProvider = telegramCommandArgsDestroyerProvider;
    }

    public T? Get<T>() where T : struct
    {
        var key = GetKey<T>();
        return _cache.GetOrDefault<T?>(key, default);
    }

    public T GetRequired<T>() where T : struct
    {
        return Get<T>() 
            ?? throw new InvalidOperationException($"Required argument of type {typeof(T).FullName} was null for user: {_httpContextAccessor.GetTelegramUserId()}");
    }

    public void Set<T>(T value)
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        var key = GetKey<T>(userId);
        var tags = GetTags(userId);
        _cache.Set(key, value, options => options.SetDurationInfinite(), tags);
    }

    public async ValueTask ClearArgumentsAsync(string commandName)
    {
        var destroyer = _telegramCommandArgsDestroyerProvider.GetDestroyer(commandName);
        if (destroyer != null)
        {
            await destroyer.DestroyAsync();
        }

        var tags = GetTags();
        await _cache.RemoveByTagAsync(tags);
    }

    private string GetKey<T>()
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        return GetKey<T>(userId);
    }

    private static string GetKey<T>(long userId)
    {
        var type = typeof(T);
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        return $"{userId}.{underlyingType.FullName}";
    }

    private IReadOnlyCollection<string> GetTags()
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        return GetTags(userId);
    }

    private static IReadOnlyCollection<string> GetTags(long userId)
    {
        return [userId.ToString()];
    }
}
