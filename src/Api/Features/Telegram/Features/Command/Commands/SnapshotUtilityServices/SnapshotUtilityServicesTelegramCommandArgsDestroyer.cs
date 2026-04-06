using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices.Arguments;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Infrastructure.FileSystem.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommandArgsDestroyer : ITelegramCommandArgsDestroyer
{
    private readonly IFileBufferService _fileBufferService;
    private readonly ICommandArgumentService _commandArgumentService;

    public SnapshotUtilityServicesTelegramCommandArgsDestroyer(
        IFileBufferService fileSBufferService,
        ICommandArgumentService commandArgumentService
        )
    {
        _fileBufferService = fileSBufferService;
        _commandArgumentService = commandArgumentService;
    }

    public async ValueTask DestroyAsync()
    {
        var tasks = new List<Task>();

        AddTask(_commandArgumentService.Get<HotWaterCounterImageCommandArg>(), tasks);
        AddTask(_commandArgumentService.Get<ColdWaterCounterImageCommandArg>(), tasks);
        AddTask(_commandArgumentService.Get<ElectricityCounterImageCommandArg>(), tasks);
        AddTask(_commandArgumentService.Get<UtilityServicesBillImageCommandArg>(), tasks);
        AddTask(_commandArgumentService.Get<CommunityServicesBillImageCommandArg>(), tasks);

        await Task.WhenAll(tasks);
    }

    private void AddTask(string? path, List<Task> tasks)
    {
        if (!string.IsNullOrEmpty(path))
        {
            tasks.Add(_fileBufferService.DeleteFileAsync(path));
        }
    }
}