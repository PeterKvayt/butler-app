
using Api.Features.Telegram.Features.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Api.Features.Telegram.Features.Security.EndpointFilters;

internal sealed class TelegramWebhookProtectionEndpointFilter(IOptions<TelegramOptions> options) : IEndpointFilter
{
    private readonly TelegramOptions _options = options.Value;

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var requestSecret)
            || !_options.WebhookSecret.Equals(requestSecret, StringComparison.Ordinal))
        {
            var forbidresult = Results.Forbid();
            return ValueTask.FromResult((object?)forbidresult);
        }

        return next(context);
    }
}
