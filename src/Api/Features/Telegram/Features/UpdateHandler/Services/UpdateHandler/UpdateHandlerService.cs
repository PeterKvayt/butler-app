using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using System.Text.Json;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;

internal sealed partial class UpdateHandlerService(IEnumerable<ITelegramUpdateHandler> telegramUpdateHandlers,
    ILogger<UpdateHandlerService> logger)
    : IUpdateHandlerService
{
    private readonly IEnumerable<ITelegramUpdateHandler> _telegramUpdateHandlers = telegramUpdateHandlers;
    private readonly ILogger<UpdateHandlerService> _logger = logger;

    public async Task HandleAsync(Update update)
    {
        var handler = _telegramUpdateHandlers.FirstOrDefault(e => e.IsSupported(update));

        if (handler == null)
        {
            var sereilizedUpdate = JsonSerializer.Serialize(update);

            _logger.LogUnsupportedUpdateType(sereilizedUpdate);

            return;
        }

        await handler.HandleAsync(update);
    }
}

internal static partial class UpdateHandlerServiceLoggerExtensions
{
    [LoggerMessage(LogLevel.Information, Message = "Unsupported update: {update}")]
    internal static partial void LogUnsupportedUpdateType(this ILogger<UpdateHandlerService> logger, string update);
}
