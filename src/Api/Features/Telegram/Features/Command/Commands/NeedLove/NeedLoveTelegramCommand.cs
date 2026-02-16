using Api.Features.Telegram.Features.Command.Abstractions;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.NeedLove;

internal sealed class NeedLoveTelegramCommand(ITelegramBotClient telegramBotClient) : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public async ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        if (commandArgs is not NeedLoveTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{commandArgs.GetType().Name} is not supported");
        }

        await _telegramBotClient.SendMessage(args.ChatId, $"Dear Lapochka, {GetRandomCompliment()}");
    }

    private static readonly string[] Compliments =
    [
        "you light up every room you enter.",
        "your smile makes my day better.",
        "you're more beautiful than you realize.",
        "you make everything feel easier.",
        "i'm lucky to have you in my life.",
        "your laugh is my favorite sound.",
        "you inspire me to be better.",
        "you make ordinary moments special.",
        "you have the kindest heart.",
        "you always know how to cheer me up.",
        "you’re incredibly thoughtful.",
        "being with you feels like home.",
        "you’re stunning inside and out.",
        "you make my world brighter.",
        "your confidence is attractive.",
        "you make every day worth it.",
        "you’re my favorite person.",
        "your kindness amazes me.",
        "you’re stronger than you think.",
        "you make life more fun.",
        "you have amazing style.",
        "you’re incredibly smart.",
        "you make people feel comfortable.",
        "your energy is contagious.",
        "you’re unforgettable.",
        "you make me smile without trying.",
        "you have the best personality.",
        "you’re absolutely charming.",
        "you make everything better.",
        "you’re beautiful in every way.",
        "you make me feel lucky.",
        "your voice is soothing.",
        "you’re effortlessly amazing.",
        "you brighten my worst days.",
        "you make my heart happy.",
        "you’re incredibly caring.",
        "you make people feel valued.",
        "you’re one of a kind.",
        "you make love feel easy.",
        "you’re full of warmth.",
        "you’re a wonderful listener.",
        "you make life exciting.",
        "you’re truly special.",
        "you make every moment better.",
        "you’re my happy place.",
        "you’re unbelievably cute.",
        "you’re naturally beautiful.",
        "you make people smile.",
        "you’re incredibly talented.",
        "you’re perfect just as you are.",
        "you make me proud.",
        "you make life sweeter.",
        "you’re amazing company.",
        "you make challenges easier.",
        "you’re so easy to love.",
        "you have a beautiful soul.",
        "you’re incredibly lovable.",
        "you make me feel safe.",
        "you make every day brighter.",
        "you’re wonderfully unique.",
        "you have a radiant smile.",
        "you make people feel important.",
        "you’re incredibly thoughtful.",
        "you make life more colorful.",
        "you’re a joy to be around.",
        "you’re simply wonderful.",
        "you bring out the best in me.",
        "you make my heart race.",
        "you’re pure happiness.",
        "you’re effortlessly charming.",
        "you make every place better.",
        "you’re truly inspiring.",
        "you’re beautiful when you laugh.",
        "you make every memory better.",
        "you’re incredibly supportive.",
        "you’re the highlight of my day.",
        "you’re full of positive energy.",
        "you make life feel lighter.",
        "you’re the reason i smile more.",
        "you make everything feel possible.",
        "you’re adorable.",
        "you make people feel loved.",
        "you’re genuinely amazing.",
        "you make hard days easier.",
        "you’re my favorite distraction.",
        "you’re incredibly sweet.",
        "you make every conversation fun.",
        "you’re beautiful in every mood.",
        "you’re truly unforgettable.",
        "you make love feel magical.",
        "you’re always on my mind.",
        "you make me laugh the most.",
        "you’re my best decision.",
        "you make everything feel right.",
        "you’re the best part of my day.",
        "you’re unbelievably special.",
        "you make every day brighter.",
        "you’re perfect to me.",
        "you make my life happier.",
        "i love everything about you."
    ];

    private static readonly Random Rand = new();

    public static string GetRandomCompliment()
    {
        var index = Rand.Next(Compliments.Length);
        return Compliments[index];
    }
}
