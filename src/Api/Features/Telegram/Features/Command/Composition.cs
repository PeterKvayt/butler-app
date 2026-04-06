using Api.Features.Telegram.Features.Command.Commands.Cancel;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Models;
using Api.Features.Telegram.Features.Command.Commands.NeedLove;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;
using Api.Features.Telegram.Features.Command.Extensions;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommand;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;
using Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsDestroyer;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Features.Telegram.Features.Command.Services.CurrentCommand;
using Api.Features.Telegram.Features.Commands.Constants;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ML;

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
            .AddScoped<ICommandArgumentService, CommandArgumentService>()
            .AddScoped<ICurrentCommandService, CurrentCommandService>()
            .AddCommandInfoProvider();

        builder.Services.TryAddSingleton(TimeProvider.System);

        var mlModelPath = Path.Combine(Environment.CurrentDirectory, "Features", "Telegram", "Features", "Command", "Commands", "ClassifyImage", "image-classifier-model--2026-03-22T18-29-16.zip");
        builder.Services
            .AddCommand<ClassifyImageTelegramCommand, ClassifyImageTelegramCommandArgsBuilder, ClassifyImageTelegramCommandArgsDestroyer>(CommandNames.ClassifyImage)
            .AddPredictionEnginePool<ImageData, ImagePrediction>().FromFile(mlModelPath);

        return builder;
    }
}
