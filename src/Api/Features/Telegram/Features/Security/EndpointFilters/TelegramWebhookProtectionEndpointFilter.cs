
namespace Api.Features.Telegram.Features.Security.EndpointFilters;

internal sealed class TelegramWebhookProtectionEndpointFilter(IConfiguration configuration) : IEndpointFilter
{
    private readonly string _webhookSecret = configuration.GetValue<string>("Telegram:WebhookSecret")
        ?? throw new InvalidOperationException("Telegram:WebhookSecret is null");

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var requestSecret)
            || !_webhookSecret.Equals(requestSecret, StringComparison.Ordinal))
        {
            var forbidresult = Results.Forbid();
            return ValueTask.FromResult((object?)forbidresult);
        }

        return next(context);
    }
}
