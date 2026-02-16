using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Shared.CommandArgs;

namespace Api.Features.Telegram.Features.Command.Commands.Shared.CommandBuilders;

internal sealed class EmptyTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private static readonly EmptyTelegramCommandArgs _args = new();

    public ITelegramCommandArgs Arguments { get; set; } = _args;

    public bool IsArgumentsFilledIn() => true;

    public Task RequestNextAgrumentAsync() => Task.CompletedTask;
}
