using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Services.CommandContext;

internal sealed class CommandContextService : ICommandContextService
{
    private static readonly Dictionary<long, CommandContextModel> _contexts = [];

    private readonly ILogger<CommandContextService> _logger;

    public CommandContextService(ILogger<CommandContextService> logger)
    {
        _logger = logger;
    }

    public CommandContextModel? GetContext(long userId)
    {
        return _contexts.GetValueOrDefault(userId);
    }

    public void AddContext(long userId, CommandContextModel context)
    {
        _contexts.Add(userId, context);
    }

    public void RemoveContext(long userId)
    {
        if (_contexts.Remove(userId))
        {
            // TODO: improve logging
            _logger.LogWarning("CommandContextModel has not been removed for {UserId}", userId);
        }
    }
}
