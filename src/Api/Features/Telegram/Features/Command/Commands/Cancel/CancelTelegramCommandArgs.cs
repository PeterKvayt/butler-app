using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Shared.CommandArgs;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed record CancelTelegramCommandArgs : BaseTelegramCommandArgs
{
    internal long? ChatId { get; set; }
    internal long GetChatId() => GetRequiredValue(ChatId);
}
