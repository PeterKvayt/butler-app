namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;

internal sealed record ChatTelegramCommandArg
{
    public long Id;

    public static implicit operator ChatTelegramCommandArg(long id) => new() { Id = id };
    public static implicit operator long?(ChatTelegramCommandArg? arg) => arg?.Id;
}
