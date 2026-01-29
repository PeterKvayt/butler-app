using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Ping;

internal static class Endpoint
{
    internal static void AddPingEndpoint(this IEndpointRouteBuilder builder) 
        => builder.MapGet("/ping", Handle);

    private static IResult Handle([FromServices] ILogger<Program> logger)
    {
        logger.LogInformation("Ping triggered");

        return Results.Ok("Pong");
    }
}