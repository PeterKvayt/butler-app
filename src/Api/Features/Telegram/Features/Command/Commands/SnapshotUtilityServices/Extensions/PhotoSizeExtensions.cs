using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Extensions;

internal static class PhotoSizeExtensions
{
    public static PhotoSize? GetHighestPhotoSize(this IEnumerable<PhotoSize> photoSizes) => photoSizes.LastOrDefault();
}
