using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;
using Api.Features.Telegram.Features.Command.Models;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Features.Telegram.Features.Commands.Constants;
using Api.Infrastructure.FileSystem.Abstractions;
using Telegram.Bot;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommand : ITelegramCommand
{
    private readonly TimeProvider _timeProvider;
    private readonly ICommandArgumentService _commandArgumentService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileBufferService _fileBufferService;

    public SnapshotUtilityServicesTelegramCommand(
        TimeProvider timeProvider,
        ICommandArgumentService commandArgumentService,
        ITelegramBotClient telegramBotClient, 
        IFileSystemService fileSystemService,
        IFileBufferService fileBufferService)
    {
        _timeProvider = timeProvider;
        _commandArgumentService = commandArgumentService;
        _telegramBotClient = telegramBotClient;
        _fileSystemService = fileSystemService;
        _fileBufferService = fileBufferService;
    }

    public CommandInfo CommandInfo { get; } = new CommandInfo
    {
        Name = CommandNames.SnapshotUtilityServices,
        Description = "Snapshot utility services"
    };

    public async ValueTask ExecuteAsync()
    {
        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var utcLastMonth = utcNow.AddMonths(-1);

        var basePath = $"/Коммуналка";

        await Task.WhenAll(
            ProcessFileAsync(_commandArgumentService.Get<ColdWaterCounterImageCommandArg>(), $"{basePath}/{utcNow:yyyy}/{utcNow:MM} холодная вода"),
            ProcessFileAsync(_commandArgumentService.Get<HotWaterCounterImageCommandArg>(), $"{basePath}/{utcNow:yyyy}/{utcNow:MM} горячая вода"),
            ProcessFileAsync(_commandArgumentService.Get<ElectricityCounterImageCommandArg>(), $"{basePath}/{utcNow:yyyy}/{utcNow:MM} электричество"),
            ProcessFileAsync(_commandArgumentService.Get<UtilityServicesBillImageCommandArg>(), $"{basePath}/{utcLastMonth:yyyy}/{utcLastMonth:MM} жкх"),
            ProcessFileAsync(_commandArgumentService.Get<CommunityServicesBillImageCommandArg>(), $"{basePath}/{utcLastMonth:yyyy}/{utcLastMonth:MM} товарищество")
        );

        await _telegramBotClient.SendMessage(_commandArgumentService.GetRequired<ChatTelegramCommandArg>().Id, "Utility services saved to disk");
    }

    private async Task ProcessFileAsync(string? bufferFilePath, string targetPath)
    {
        if (bufferFilePath == null)
        {
            return;
        }

        var extension = Path.GetExtension(bufferFilePath);
        var targetPathWithExtension = $"{targetPath}{extension}";

        using var file = await _fileBufferService.GetFileAsync(bufferFilePath);

        await _fileSystemService.SaveFileAsync(file, targetPathWithExtension);
    }
}
