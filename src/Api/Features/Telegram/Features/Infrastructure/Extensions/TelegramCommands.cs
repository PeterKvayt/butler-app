using Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Infrastructure.Extensions;

internal static class TelegramCommands
{
    // docs: https://core.telegram.org/bots/api#setmycommands

    internal static async Task SetupTelegramCommandsAsync(this WebApplication app)
    {
        var commandInfosProvider = app.Services.GetRequiredService<ITelegramCommandInfoProvider>();
        var botCommands = commandInfosProvider.CommandInfos.Select(e => new BotCommand(e.Name, e.Description));
        var client = app.Services.GetRequiredService<ITelegramBotClient>();
        await client.SetMyCommands(botCommands);
    }
}
