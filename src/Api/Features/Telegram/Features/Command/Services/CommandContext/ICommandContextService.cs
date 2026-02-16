using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Services.CommandContext;

internal interface ICommandContextService
{
    internal void RemoveContext(long userId);
    internal CommandContextModel GetOrAddContext(long userId, Func<long, CommandContextModel> contextFactory);
}