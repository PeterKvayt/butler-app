using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder
{
    internal interface ITelegramCommandArgsBuilderProvider
    {
        internal ITelegramCommandArgsBuilder GetBuilder(CommandContextModel commandContextModel);
        internal ITelegramCommandArgsBuilder GetBuilder(string commandName);
    }
}