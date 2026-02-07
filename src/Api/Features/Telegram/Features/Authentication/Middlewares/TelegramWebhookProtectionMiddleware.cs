
namespace Api.Features.Telegram.Features.Authentication.Middlewares;

internal class TelegramWebhookProtectionMiddleware(IConfiguration configuration) : IMiddleware
{
    private readonly string _webhookSecret = configuration.GetValue<string>("Telegram:WebhookSecret") 
        ?? throw new InvalidOperationException("Telegram:WebhookSecret is null");

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Path.StartsWithSegments($"/{UpdateHandler.Endpoint.Segment}"))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var requestSecret)
            || !_webhookSecret.Equals(requestSecret, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await next(context);
    }
}
