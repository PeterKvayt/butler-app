using Api.Features.Telegram.Features.Command.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.Shared.CommandArgs;

internal abstract record BaseTelegramCommandArgs : ITelegramCommandArgs
{
    protected static long GetRequiredValue(long? value) => value ?? throw new ArgumentNullException("The required value is null");
    protected static T GetRequiredValue<T>(T? value) => value ?? throw new ArgumentNullException("The required value is null");
}
