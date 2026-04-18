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
            DeleteFileFromBuffer<HotWaterCounterImageCommandArg>(),
            DeleteFileFromBuffer<ColdWaterCounterImageCommandArg>(),
            DeleteFileFromBuffer<ElectricityCounterImageCommandArg>(),
            DeleteFileFromBuffer<UtilityServicesBillImageCommandArg>(),
            DeleteFileFromBuffer<CommunityServicesBillImageCommandArg>()
        );
    }

    private Task DeleteFileFromBuffer<T>() where T: IPathCommandArg
    {
        var arg = _commandArgumentService.Get<T>();
        if (arg == null)
        {
            return Task.CompletedTask;
        }

        return _fileBufferService.DeleteFileAsync(arg.Path);
    }
}