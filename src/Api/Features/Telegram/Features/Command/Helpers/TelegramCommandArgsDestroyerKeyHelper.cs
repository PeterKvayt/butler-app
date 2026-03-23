namespace Api.Features.Telegram.Features.Command.Helpers;

internal static class TelegramCommandArgsDestroyerKeyHelper
{
    internal static string CreateKey(string commandName) => $"TelegramCommandArgsDestroyer.{commandName}";
}
