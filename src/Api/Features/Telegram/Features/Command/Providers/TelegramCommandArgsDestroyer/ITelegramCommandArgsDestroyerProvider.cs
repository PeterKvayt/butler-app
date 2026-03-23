using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer
{
    internal interface ITelegramCommandArgsDestroyerProvider
    {
        internal ITelegramCommandArgsDestroyer? GetDestroyer(string commandName);
    }
}