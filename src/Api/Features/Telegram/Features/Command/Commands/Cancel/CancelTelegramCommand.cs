using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Services.CommandContext;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed class CancelTelegramCommand(
    ICommandContextService commandContextService,
    IHttpContextAccessor httpContextAccessor) : ITelegramCommand
{
    private readonly ICommandContextService _commandContextService = commandContextService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        _commandContextService.RemoveContext(userId);

        return ValueTask.CompletedTask;
    }
}
