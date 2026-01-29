namespace Api.Features.TelegramTest.Services.UpdateHandler;

internal interface IUpdateHandlerService
{
    internal Task HandleAsync(Telegram.Bot.Types.Update update);
}