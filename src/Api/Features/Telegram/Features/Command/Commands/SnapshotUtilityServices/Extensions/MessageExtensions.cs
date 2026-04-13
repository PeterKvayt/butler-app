using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;

internal static class MessageExtensions
{
    public static string? GetFileId(this Message message)
    {
        return message.Type switch
        {
            MessageType.Photo => message.Photo?.GetHighestPhotoSize()?.FileId,
            MessageType.Document => message.Document?.FileId,
            _ => null
        };
    }

    public static string GetRequiredFileId(this Message message)
    {
        return GetFileId(message) ?? throw new InvalidOperationException("FileId is null"); ;
    }
}
