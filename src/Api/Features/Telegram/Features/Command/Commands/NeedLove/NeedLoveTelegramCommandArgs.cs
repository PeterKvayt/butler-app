using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.NeedLove;

internal sealed record NeedLoveTelegramCommandArgs : ITelegramCommandArgs
{
    internal long? ChatId { get; set; }

    internal long GetChatId() => ChatId ?? throw new InvalidOperationException($"{nameof(ChatId)} is null");
}
