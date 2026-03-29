namespace Api.Infrastructure.Cache;

internal static class Composition
{
    internal static WebApplicationBuilder AddCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddFusionCache();

        return builder;
    }
}
