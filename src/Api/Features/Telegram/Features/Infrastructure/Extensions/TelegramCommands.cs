using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Infrastructure.Extensions;

internal static class TelegramCommands
{
    // docs: https://core.telegram.org/bots/api#setmycommands

    internal static async Task SetupTelegramCommandsAsync(this WebApplication app)
    {
        var client = app.Services.GetRequiredService<ITelegramBotClient>();

        var commands = new List<BotCommand>
        {

        };

        await client.SetMyCommands(commands);
    }
}
