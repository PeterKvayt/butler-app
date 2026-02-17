namespace Api.Features.Telegram.Features.Command.Helpers;

internal static class TelegramCommandKeyHelper
{
    internal static string CreateKey(string commandName) => $"TelegramCommand.{commandName}";
}
