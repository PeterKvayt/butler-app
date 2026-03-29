using Api.Features.Telegram.Features.Authentication.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Api.Features.Telegram.Features.Command.Services.CurrentCommand;

internal sealed class CurrentCommandService: ICurrentCommandService
{
    private readonly IFusionCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentCommandService(IFusionCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetOrCreate(string? commandName)
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        return _cache.GetOrSet<string?>(
            GetKey(userId),
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

    public void Remove()
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        _cache.Remove(GetKey(userId));
    }

    private static string GetKey(long userId) => $"CurrentCommand.{userId}";
}
