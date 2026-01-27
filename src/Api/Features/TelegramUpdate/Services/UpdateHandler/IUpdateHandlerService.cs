using Telegram.Bot.Types;

namespace Api.Features.TelegramUpdate.Services.UpdateHandler;

internal interface IUpdateHandlerService
{
    internal Task HandleAsync(Update update);
}