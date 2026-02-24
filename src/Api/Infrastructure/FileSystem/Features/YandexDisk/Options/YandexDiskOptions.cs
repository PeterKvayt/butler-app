namespace Api.Infrastructure.FileSystem.Features.YandexDisk.Options;

public sealed record YandexDiskOptions
{
    /// <summary>
    /// Yandex oauth access token
    /// </summary>
    /// <remarks>
    /// It is temporary and will expire after certain period of time.
    /// Read more here: <see href="https://yandex.ru/dev/id/doc/ru/concepts/ya-oauth-intro"/>, <see href="https://yandex.ru/dev/id/doc/ru/tokens/debug-token"/>
    /// </remarks>
    public required string AccessToken { get; init; }
}
