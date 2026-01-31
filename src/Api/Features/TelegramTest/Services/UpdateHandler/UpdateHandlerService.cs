using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.TelegramTest.Services.UpdateHandler;

internal sealed class UpdateHandlerService(ITelegramBotClient client, 
    ILogger<UpdateHandlerService> logger)
    : IUpdateHandlerService
{
    private readonly ITelegramBotClient _client = client;
    private readonly ILogger<UpdateHandlerService> _logger = logger;

    public async Task HandleAsync(Update update)
    {
        var serialized = JsonSerializer.Serialize(update);
        _logger.LogDebug(serialized);

        var type = update.Type;
        
        if (update.Message != null)
        {
            await _client.SendMessage(update.Message.Chat.Id, $"Response from server! The type is: {type}");
        }
    }
}
