using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;
using ZiggyCreatures.Caching.Fusion;

namespace Api.Features.Telegram.Features.Command.Services.CurrentCommand;

internal sealed class CurrentCommandService: ICurrentCommandService
{
    private readonly IFusionCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITelegramCommandArgsDestroyerProvider _telegramCommandArgsDestroyerProvider;

    public CurrentCommandService(
        IFusionCache cache, 
        IHttpContextAccessor httpContextAccessor,
        ITelegramCommandArgsDestroyerProvider telegramCommandArgsDestroyerProvider)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _telegramCommandArgsDestroyerProvider = telegramCommandArgsDestroyerProvider;
    }

    public string? GetOrCreate(string? commandName)
    {
        return _cache.GetOrSet<string?>(
            GetKey(),
            (ctx, ct) =>
            {
                if (commandName == null)
                {
                    ctx.Options.Duration = TimeSpan.FromMilliseconds(1);
                }
                return commandName;
            },
            options => options.SetDurationInfinite()
        );
    }

    public async ValueTask RemoveAsync()
    {
        var key = GetKey();
        await _cache.RemoveAsync(key);
    }

    private string GetKey()
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        return $"CurrentCommand.{userId}";
    }
}
