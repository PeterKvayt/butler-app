using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Shared.CommandArgs;
using Api.Features.Telegram.Features.Command.Services.CommandContext;

namespace Api.Features.Telegram.Features.Command.Commands.NeedLove;

internal sealed class NeedLoveTelegramCommand(
    ICommandContextService commandContextService,
    IHttpContextAccessor httpContextAccessor) : ITelegramCommand
{
    private readonly ICommandContextService _commandContextService = commandContextService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        if (commandArgs is not EmptyTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{commandArgs.GetType().Name} is not supported");
        }

        var userId = _httpContextAccessor.GetTelegramUserId();
        _commandContextService.RemoveContext(userId);

        return ValueTask.CompletedTask;
    }
}
