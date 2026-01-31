using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Ping;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder AddPingEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/ping", Handle);
        
        return builder;
    }

    private static IResult Handle([FromServices] ILogger<Program> logger)
    {
        logger.LogInformation("Ping triggered");

        return Results.Ok("Pong");
    }
}