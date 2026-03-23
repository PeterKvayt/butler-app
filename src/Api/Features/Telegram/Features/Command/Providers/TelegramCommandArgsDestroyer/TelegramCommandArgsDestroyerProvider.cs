using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Helpers;
namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;

internal sealed class TelegramCommandArgsDestroyerProvider : ITelegramCommandArgsDestroyerProvider
{
    private readonly IServiceProvider _serviceProvider;

    public TelegramCommandArgsDestroyerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITelegramCommandArgsDestroyer? GetDestroyer(string commandName)
    {
        var key = TelegramCommandArgsDestroyerKeyHelper.CreateKey(commandName);
        return _serviceProvider.GetKeyedService<ITelegramCommandArgsDestroyer>(key);
    }
}
