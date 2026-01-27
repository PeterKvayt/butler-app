using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.TelegramUpdate.Services.UpdateHandler;

internal sealed class UpdateHandlerService(ITelegramBotClient client): IUpdateHandlerService
{
    private readonly ITelegramBotClient _client = client;

    public async Task HandleAsync(Update update)
    {
        var type = update.Type;
        
        await _client.SendMessage(update.Message.Chat.Id, $"Response from server! The type is: {type}");
    }
}
