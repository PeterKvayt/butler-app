using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Services.TelegramUpdate;

internal interface ITelegramUpdateService
{
    internal Task ProcessUpdateAsync(Update update);
}