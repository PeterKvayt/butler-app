using Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Extensions;

internal static class MessageExtensions
{
    public static string? GetCommandName(this Message message, ITelegramCommandInfoProvider telegramCommandInfoProvider)
    {
        var commandText = message.Text?.TrimStart('/');

        if (commandText == null)
        {
            return null;
        }

        var commandName = telegramCommandInfoProvider.CommandInfos
            .Select(e => e.Name)
            .FirstOrDefault(e => e == commandText);

        return commandName;
    }
}
