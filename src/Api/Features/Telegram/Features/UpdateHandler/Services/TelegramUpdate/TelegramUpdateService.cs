using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using Api.Features.Telegram.Features.UpdateHandler.Services.TelegramUpdate;
using System.Text.Json;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;

internal sealed partial class TelegramUpdateService(IEnumerable<ITelegramUpdateHandler> telegramUpdateHandlers,
    ILogger<TelegramUpdateService> logger)
    : ITelegramUpdateService
{
    private readonly IEnumerable<ITelegramUpdateHandler> _telegramUpdateHandlers = telegramUpdateHandlers;
    private readonly ILogger<TelegramUpdateService> _logger = logger;

    public async Task ProcessUpdateAsync(Update update)
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
    internal static partial void LogUnsupportedUpdateType(this ILogger<TelegramUpdateService> logger, string update);
}
