using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.UpdateHandler.Services.UpdateHandler;

internal interface IUpdateHandlerService
{
    internal Task HandleAsync(Update update);
}