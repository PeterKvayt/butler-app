using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Helpers;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandInfo;

namespace Api.Features.Telegram.Features.Command.Extensions;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddCommand<TCommand, TCommandArgsBuilder>(this IServiceCollection services, string commandName)
        where TCommand : class, ITelegramCommand
        where TCommandArgsBuilder: class, ITelegramCommandArgsBuilder
    {
        services.AddScoped<TCommand>();
        services.AddScoped<ITelegramCommand, TCommand>(e => e.GetRequiredService<TCommand>());
        services.AddKeyedScoped<ITelegramCommand, TCommand>(TelegramCommandKeyHelper.CreateKey(commandName), (e, key) => e.GetRequiredService<TCommand>());

        services.AddKeyedScoped<ITelegramCommandArgsBuilder, TCommandArgsBuilder>(TelegramCommandArgsBuilderKeyHelper.CreateKey(commandName));

        return services;
    }

    internal static IServiceCollection AddCommandInfoProvider(this IServiceCollection services)
    {
        services.AddSingleton<ITelegramCommandInfoProvider>(serviceProvider =>
        {
            using var scope = serviceProvider.CreateScope();

            var commandInfos = scope.ServiceProvider
                .GetServices<ITelegramCommand>()
                .Select(e => e.CommandInfo)
                .ToArray();
            return new TelegramCommandInfoProvider(commandInfos);
        });

        return services;
    }
}
