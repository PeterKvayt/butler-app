using Api.Features.Telegram.Features.Command.Models;
using System.Collections.Concurrent;

namespace Api.Features.Telegram.Features.Command.Services.CommandContext;

internal sealed class CommandContextService(ILogger<CommandContextService> logger) : ICommandContextService
{
    private readonly ILogger<CommandContextService> _logger = logger;
    private static readonly ConcurrentDictionary<long, CommandContextModel> _contexts = new();

    public CommandContextModel GetOrAddContext(long userId, Func<long, CommandContextModel> contextFactory)
    {
        return _contexts.GetOrAdd(userId, (userId) =>
        {
            return contextFactory(userId);
        });
    }

    public void RemoveContext(long userId)
    {
        if (_contexts.TryRemove(userId, out var _))
        {
            // TODO: improve logging
            _logger.LogWarning("CommandContextModel has not been removed for {UserId}", userId);
        }
    }
}
