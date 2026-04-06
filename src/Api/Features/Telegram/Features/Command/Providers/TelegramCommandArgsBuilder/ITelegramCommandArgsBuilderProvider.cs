using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder
{
    internal interface ITelegramCommandArgsBuilderProvider
    {
        internal ITelegramCommandArgsBuilder GetBuilder(string commandName);
    }
}