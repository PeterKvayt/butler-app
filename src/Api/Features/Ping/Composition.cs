namespace Api.Features.Ping;

internal static class Composition
{
    internal static WebApplication UsePing(this WebApplication app)
    {
        app.AddPingEndpoint();

        return app;
    }
}