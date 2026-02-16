namespace Api.Features.Telegram.Features.Command.Abstractions;

internal interface ITelegramCommandArgsBuilder
{
    internal ITelegramCommandArgs Arguments { get; set; }
    internal Task RequestNextAgrumentAsync();
    internal bool IsArgumentsFilledIn();
}
