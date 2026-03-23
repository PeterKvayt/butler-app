using Api.Features.Telegram.Features.Command.Commands.Cancel;
using Api.Features.Telegram.Features.Command.Commands.NeedLove;
using Api.Features.Telegram.Features.Command.Extensions;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommand;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;
using Api.Features.Telegram.Features.Command.Services.CommandContext;
using Api.Features.Telegram.Features.Commands.Constants;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.Features.Telegram.Features.Command;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramCommands(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCommand<CancelTelegramCommand, CancelTelegramCommandArgsBuilder>(CommandNames.Cancel)
            .AddCommand<NeedLoveTelegramCommand, NeedLoveTelegramCommandArgsBuilder>(CommandNames.NeedLove)
            .AddCommand<SnapshotUtilityServicesTelegramCommand, SnapshotUtilityServicesTelegramCommandArgsBuilder, SnapshotUtilityServicesTelegramCommandArgsDestroyer>(CommandNames.SnapshotUtilityServices)
            ;
        
        builder.Services
            .AddScoped<ITelegramCommandProvider, TelegramCommandProvider>()
            .AddScoped<ITelegramCommandArgsBuilderProvider, TelegramCommandArgsBuilderProvider>()
            .AddScoped<ITelegramCommandArgsDestroyerProvider, TelegramCommandArgsDestroyerProvider>()
            .AddScoped<ICommandContextService, CommandContextService>()
            .AddCommandInfoProvider();

        builder.Services.TryAddSingleton(TimeProvider.System);

        return builder;
    }
}
