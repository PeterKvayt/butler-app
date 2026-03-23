using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Commands.Constants;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommand : ITelegramCommand
{
    private readonly TimeProvider _timeProvider;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileBufferService _fileBufferService;

    public SnapshotUtilityServicesTelegramCommand(
        TimeProvider timeProvider,
        ITelegramBotClient telegramBotClient, 
        IFileSystemService fileSystemService,
        IFileBufferService fileBufferService)
    {
        _timeProvider = timeProvider;
        _telegramBotClient = telegramBotClient;
        _fileSystemService = fileSystemService;
        _fileBufferService = fileBufferService;
    }

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.SnapshotUtilityServices,
        Description = "Snapshot utility services"
    };

    public async ValueTask ExecuteAsync(ITelegramCommandArgs commandArgs)
    {
        if (commandArgs is not SnapshotUtilityServicesTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{commandArgs.GetType().Name} is not supported");
        }

        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var utcLastMonth = utcNow.AddMonths(-1);

        var basePath = $"/Коммуналка";

        await Task.WhenAll(
            ProcessFileAsync(args.GetColdWaterCounterFilePath(), $"{basePath}/{utcNow:yyyy}/{utcNow:MM} холодная вода"),
            ProcessFileAsync(args.GetHotWaterCounterFilePath(), $"{basePath}/{utcNow:yyyy}/{utcNow:MM} горячая вода"),
            ProcessFileAsync(args.GetElectricityCounterFilePath(), $"{basePath}/{utcNow:yyyy}/{utcNow:MM} электричество"),
            ProcessFileAsync(args.GetUtilityServicesBillFilePath(), $"{basePath}/{utcLastMonth:yyyy}/{utcLastMonth:MM} жкх"),
            ProcessFileAsync(args.GetCommunityServicesBillFilePath(), $"{basePath}/{utcLastMonth:yyyy}/{utcLastMonth:MM} товарищество")
        );

        await _telegramBotClient.SendMessage(args.GetChatId(), "Utility services saved to disk");
    }

    private async Task ProcessFileAsync(string bufferFilePath, string targetPath)
    {
        var extension = Path.GetExtension(bufferFilePath);
        var targetPathWithExtension = $"{targetPath}{extension}";

        using var file = await _fileBufferService.GetFileAsync(bufferFilePath);

        await _fileSystemService.SaveFileAsync(file, targetPathWithExtension);
    }
}
