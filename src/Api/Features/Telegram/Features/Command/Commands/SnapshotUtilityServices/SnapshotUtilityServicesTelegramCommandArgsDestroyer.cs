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
        await Task.WhenAll(
            DeleteFileFromBuffer(_commandArgumentService.Get<HotWaterCounterImageCommandArg>()),
            DeleteFileFromBuffer(_commandArgumentService.Get<ColdWaterCounterImageCommandArg>()),
            DeleteFileFromBuffer(_commandArgumentService.Get<ElectricityCounterImageCommandArg>()),
            DeleteFileFromBuffer(_commandArgumentService.Get<UtilityServicesBillImageCommandArg>()),
            DeleteFileFromBuffer(_commandArgumentService.Get<CommunityServicesBillImageCommandArg>())
        );
    }

    private Task DeleteFileFromBuffer(string? path)
    {
        if (path == null)
        {
            return Task.CompletedTask;
        }

        return _fileBufferService.DeleteFileAsync(path);
    }
}