using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.Shared.CommandArgs;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed record SnapshotUtilityServicesTelegramCommandArgs : BaseTelegramCommandArgs
{
    internal long? ChatId { get; set; }
    internal long GetChatId() => GetRequiredValue(ChatId);
    internal string? HotWaterCounterFilePath { get; set; }
    internal string GetHotWaterCounterFilePath() => GetRequiredValue(HotWaterCounterFilePath);
    internal string? ColdWaterCounterFilePath { get; set; }
    internal string GetColdWaterCounterFilePath() => GetRequiredValue(ColdWaterCounterFilePath);
    internal string? ElectricityCounterFilePath { get; set; }
    internal string GetElectricityCounterFilePath() => GetRequiredValue(ElectricityCounterFilePath);
    internal string? UtilityServicesBillFilePath { get; set; }
    internal string GetUtilityServicesBillFilePath() => GetRequiredValue(UtilityServicesBillFilePath);
    internal string? CommunityServicesBillFilePath { get; set; }
    internal string GetCommunityServicesBillFilePath() => GetRequiredValue(CommunityServicesBillFilePath);

}
