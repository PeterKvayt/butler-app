using Api.Features.Telegram.Features.Command.Commands.Shared.CommandArgs;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed record ClassifyImageTelegramCommandArgs : BaseTelegramCommandArgs
{
    internal long? ChatId { get; set; }
    internal long GetChatId() => GetRequiredValue(ChatId);
    internal string? ImagePath { get; set; }
    internal string GetImagePath() => GetRequiredValue(ImagePath);
}
