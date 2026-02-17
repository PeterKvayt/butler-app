using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;

internal interface ITelegramCommandInfoProvider
{
    internal IReadOnlySet<CommandInfo> CommandInfos { get; }
}