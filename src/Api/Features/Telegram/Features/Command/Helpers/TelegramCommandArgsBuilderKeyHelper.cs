namespace Api.Features.Telegram.Features.Command.Helpers;

internal static class TelegramCommandArgsBuilderKeyHelper
{
    internal static string CreateKey(string commandName) => $"TelegramCommandArgsBuilder.{commandName}";
}
