using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Cancel.Arguments;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed class CancelTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ICommandArgumentService _commandArgumentService;
    private ChatTelegramCommandArg? _chatArg;

    public CancelTelegramCommandArgsBuilder(
        ICommandArgumentService commandArgumentService
        )
    {
        _commandArgumentService = commandArgumentService;
    }

    public ValueTask AddAgrumentAsync(Message message)
    {
        _chatArg = _commandArgumentService.Get<ChatTelegramCommandArg>();

        if (_chatArg == null)
        {
            _chatArg = message.Chat.Id;
            _commandArgumentService.Set(_chatArg);
        }

        return ValueTask.CompletedTask;
    }

    public Task RequestNextAgrumentAsync() => Task.CompletedTask;

    public bool IsArgumentsFilledIn() => _chatArg.HasValue;
}