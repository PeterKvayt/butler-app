using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.NeedLove.Arguments;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.NeedLove;

internal sealed class NeedLoveTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ICommandArgumentService _commandArgumentService;

    public NeedLoveTelegramCommandArgsBuilder(ICommandArgumentService commandArgumentService)
    {
        _commandArgumentService = commandArgumentService;
    }

    public ValueTask AddAgrumentAsync(Message message)
    {
        if (_commandArgumentService.Get<ChatTelegramCommandArg>() == null)
        {
            _commandArgumentService.Set<ChatTelegramCommandArg>(message.Chat.Id);
        }

        return ValueTask.CompletedTask;
    }

    public Task RequestNextAgrumentAsync() => Task.CompletedTask;

    public bool IsArgumentsFilledIn() => true;
}
