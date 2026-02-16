using Api.Features.Telegram.Features.Command.Commands.Cancel;
using Api.Features.Telegram.Features.Command.Commands.NeedLove;
using Api.Features.Telegram.Features.Command.Commands.Shared.CommandBuilders;
using Api.Features.Telegram.Features.Command.Extensions;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommand;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;
using Api.Features.Telegram.Features.Command.Services.CommandContext;
using Api.Features.Telegram.Features.Commands.Constants;

namespace Api.Features.Telegram.Features.Command;

internal static class Composition
{
    internal static WebApplicationBuilder AddTelegramCommands(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCommand<CancelTelegramCommand, EmptyTelegramCommandArgsBuilder>(CommandNames.Cancel)
            .AddCommand<NeedLoveTelegramCommand, EmptyTelegramCommandArgsBuilder>(CommandNames.NeedLove)
            ;
        
        builder.Services
            .AddScoped<ITelegramCommandProvider, TelegramCommandProvider>()
            .AddScoped<ITelegramCommandArgsBuilderProvider, TelegramCommandArgsBuilderProvider>()
            .AddScoped<ICommandContextService, CommandContextService>();
        
        return builder;
    }
}
