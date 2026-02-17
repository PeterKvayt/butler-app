using Api.Features.Telegram.Features.Command.Models;

namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;

internal sealed class TelegramCommandInfoProvider : ITelegramCommandInfoProvider
{
    public IReadOnlySet<CommandInfo> CommandInfos { get; init; }

    public TelegramCommandInfoProvider(IReadOnlyCollection<CommandInfo> commandInfos)
    {
        CommandInfos = commandInfos.ToHashSet();
    }
}
