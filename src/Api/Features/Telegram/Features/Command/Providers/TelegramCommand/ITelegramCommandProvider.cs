using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommand;

internal interface ITelegramCommandProvider
{
    internal ITelegramCommand GetCommand(string commandName);
}