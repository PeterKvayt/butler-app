using Api.Features.Telegram.Features.Authentication.Extensions;
using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Services.CommandContext;
using Api.Features.Telegram.Features.Commands.Constants;

namespace Api.Features.Telegram.Features.Command.Commands.Cancel;

internal sealed class CancelTelegramCommand(
    ICommandContextService commandContextService,
    IHttpContextAccessor httpContextAccessor) : ITelegramCommand
{
    private readonly ICommandContextService _commandContextService = commandContextService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.Cancel,
        Description = "Cancel the current command"
    };

    public ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        var userId = _httpContextAccessor.GetTelegramUserId();
        _commandContextService.RemoveContext(userId);

        return ValueTask.CompletedTask;
    }
}
