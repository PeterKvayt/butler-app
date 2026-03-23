using Api.Features.Telegram.Features.Command.Abstractions;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed class CancelTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private CancelTelegramCommandArgs _args = new();

    public ITelegramCommandArgs Arguments
    {
        get => _args;
        set {
            if (value is not CancelTelegramCommandArgs args)
            {
                throw new InvalidOperationException($"Unsupported args of type {value.GetType()}");
            }

            _args = args;
        }
    }
    
    public ValueTask AddAgrumentAsync(Message message)
    {
        if (!_args.ChatId.HasValue)
        {
            _args.ChatId = message.Chat.Id;
        }

        return ValueTask.CompletedTask;
    }

    public Task RequestNextAgrumentAsync() => Task.CompletedTask;

    public bool IsArgumentsFilledIn() => _args.ChatId.HasValue;
}