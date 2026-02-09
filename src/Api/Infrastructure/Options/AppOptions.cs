namespace Api.Infrastructure.Options;

public sealed record AppOptions
{
    public required string BaseUrl { get; init; }
}
