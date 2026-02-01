using Api.Features.Telegram.Features.UpdateHandler.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.MessageProcess.Services.MessageHandle;

internal sealed class MessageHandlerService(ITelegramBotClient telegramBotClient) : ITelegramUpdateHandler
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public async ValueTask HandleAsync(Update update)
    {
        if (update.Message == null)
        {
            throw new InvalidOperationException("Message is null");
        }

        await _telegramBotClient.SendMessage(update.Message.Chat.Id, $"Message received! Nice work {update.Message.From?.FirstName}");
    }

    public bool IsSupported(Update update) => update.Type == UpdateType.Message;
}
