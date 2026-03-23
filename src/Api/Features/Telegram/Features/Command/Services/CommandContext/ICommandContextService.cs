using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Services.CommandContext;

internal interface ICommandContextService
{
    internal void AddContext(long userId, CommandContextModel context);
    internal CommandContextModel? GetContext(long userId);
    internal ValueTask RemoveContextAsync(long userId);
}