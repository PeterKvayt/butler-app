using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Helpers;

namespace Api.Features.Telegram.Features.Command.Extensions;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddCommand<TCommand, TCommandArgsBuilder>(this IServiceCollection services, string commandName)
        where TCommand : class, ITelegramCommand
        where TCommandArgsBuilder: class, ITelegramCommandArgsBuilder
    {
        services.AddKeyedScoped<ITelegramCommand, TCommand>(TelegramCommandKeyHelper.CreateKey(commandName));
        services.AddKeyedScoped<ITelegramCommandArgsBuilder, TCommandArgsBuilder>(TelegramCommandArgsBuilderKeyHelper.CreateKey(commandName));

        return services;
    }
}
