using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.UploadFile;

internal sealed record UploadFileTelegramCommandArgs : ITelegramCommandArgs
{
    internal long? ChatId { get; set; }

    internal long GetChatId() => ChatId ?? throw new InvalidOperationException($"{nameof(ChatId)} is null");

    internal string? TempFilePath { get; set; }

    internal string GetTempFilePath() => TempFilePath ?? throw new InvalidOperationException($"{nameof(TempFilePath)}");
}
