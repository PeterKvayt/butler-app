using Api.Features.Telegram.Features.Authentication.Constants;
using Api.Features.Telegram.Features.Authentication.Handlers;
using Api.Features.Telegram.Features.Authentication.Middlewares;
using Microsoft.AspNetCore.Authentication;

namespace Api.Features.Telegram.Features.Authentication;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramAuthentication(this WebApplicationBuilder builder)
    {
        var authBuilder = new AuthenticationBuilder(builder.Services);

        authBuilder.AddScheme<AuthenticationSchemeOptions, TelegramAuthenticationHandler>(AuthenticationSchemas.TelegramWebhook);

        builder.Services.AddSingleton<TelegramWebhookProtectionMiddleware>();

        return builder;
    }

    internal static WebApplication UseTelegramAuthentication(this WebApplication app)
    {
        app.UseMiddleware<TelegramWebhookProtectionMiddleware>();

        return app;
    }
}

file static class AuthenticationBuilderExtensions
{
    internal static AuthenticationBuilder AddScheme<TOptions, THandler>(this AuthenticationBuilder builder, string authenticationScheme)
        where TOptions : AuthenticationSchemeOptions, new()
        where THandler : class, IAuthenticationHandler
    {
        builder.Services.Configure<AuthenticationOptions>(o =>
        {
            o.AddScheme(authenticationScheme, scheme =>
            {
                scheme.HandlerType = typeof(THandler);
                scheme.DisplayName = authenticationScheme;
            });
        });

        builder.Services.AddOptions<TOptions>(authenticationScheme).Validate(o =>
        {
            o.Validate(authenticationScheme);
            return true;
        });

        builder.Services.AddTransient<THandler>();

        return builder;
    }
}