using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Infrastructure.FileSystem.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.SnapshotUtilityServices;

internal sealed class SnapshotUtilityServicesTelegramCommandArgsDestroyer : ITelegramCommandArgsDestroyer
{
    private readonly IFileBufferService _fileBufferService;

    private SnapshotUtilityServicesTelegramCommandArgs _args = new();
    public SnapshotUtilityServicesTelegramCommandArgsDestroyer(IFileBufferService fileSBufferService)
    {
        _fileBufferService = fileSBufferService;
    }

    public async ValueTask DestroyAsync(ITelegramCommandArgs arguments)
    {
        if (arguments is not SnapshotUtilityServicesTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{arguments.GetType().Name} is not supported");
        }

        var tasks = new List<Task>();

        AddTask(args.HotWaterCounterFilePath, tasks);
        AddTask(args.ColdWaterCounterFilePath, tasks);
        AddTask(args.ElectricityCounterFilePath, tasks);
        AddTask(args.UtilityServicesBillFilePath, tasks);
        AddTask(args.CommunityServicesBillFilePath, tasks);

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