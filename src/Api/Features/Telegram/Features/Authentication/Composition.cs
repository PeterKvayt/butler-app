using Api.Features.Telegram.Features.Authentication.AuthenticationHandler;
using Api.Features.Telegram.Features.Authentication.Constants;
using Microsoft.AspNetCore.Authentication;

namespace Api.Features.Telegram.Features.Authentication;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramAuthentication(this WebApplicationBuilder builder)
    {
        var authBuilder = new AuthenticationBuilder(builder.Services);

        authBuilder.AddScheme<AuthenticationSchemeOptions, TelegramAuthenticationHandler>(AuthenticationSchemes.TelegramWebhook);

        return builder;
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