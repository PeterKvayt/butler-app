using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Helpers;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommand;

internal sealed class TelegramCommandProvider(IServiceProvider serviceProvider) : ITelegramCommandProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ITelegramCommand GetCommand(string commandName)
    {
        var key = TelegramCommandKeyHelper.CreateKey(commandName);
        return _serviceProvider.GetRequiredKeyedService<ITelegramCommand>(key);
    }
}
