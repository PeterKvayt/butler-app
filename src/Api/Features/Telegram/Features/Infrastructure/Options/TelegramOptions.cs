namespace Api.Features.Telegram.Features.Infrastructure.Options;

public sealed record TelegramOptions
{
    public required string WebhookSecret { get; init; }
    public required string BotToken { get; init; }
    public IReadOnlySet<string> AllowedUserIds { get; init; } = new HashSet<string>();
}
