using Api.Features.Telegram.Features.Command.Abstractions;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.NeedLove;

internal sealed class NeedLoveTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private NeedLoveTelegramCommandArgs _args = new();

    public ITelegramCommandArgs Arguments
    {
        get => _args;
        set {
            if (value is not NeedLoveTelegramCommandArgs args)
            {
                throw new InvalidOperationException($"Unsupported args of type {value.GetType()}");
            }

            _args = args;
        }
    }
    
    public ValueTask AddAgrumentAsync(Message message)
    {
        _args.ChatId = message.Chat.Id;

        return ValueTask.CompletedTask;
    }

    public Task RequestNextAgrumentAsync() => Task.CompletedTask;

    public bool IsArgumentsFilledIn()
    {
        return _args.ChatId.HasValue;
    }
}
