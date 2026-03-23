using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;

namespace Api.Features.Telegram.Features.Command.Services.CommandContext;

internal sealed class CommandContextService : ICommandContextService
{
    private static readonly Dictionary<long, CommandContextModel> _contexts = [];

    private readonly ITelegramCommandArgsDestroyerProvider _telegramCommandArgsDestroyerProvider;

    public CommandContextService(
        ITelegramCommandArgsDestroyerProvider telegramCommandArgsDestroyerProvider
    )
    {
        _telegramCommandArgsDestroyerProvider = telegramCommandArgsDestroyerProvider;
    }

    public CommandContextModel? GetContext(long userId)
    {
        return _contexts.GetValueOrDefault(userId);
    }

    public void AddContext(long userId, CommandContextModel context)
    {
        _contexts.Add(userId, context);
    }

    public async ValueTask RemoveContextAsync(long userId)
    {
        var context = GetContext(userId);

        if (context == null)
        {
            return;
        }
        
        _contexts.Remove(userId);

        var argumentsDestroyer = _telegramCommandArgsDestroyerProvider.GetDestroyer(context.CommandName);

        if (argumentsDestroyer == null)
        {
            return;
        }

        await argumentsDestroyer.DestroyAsync(context.CommandArgs);
    }
}
