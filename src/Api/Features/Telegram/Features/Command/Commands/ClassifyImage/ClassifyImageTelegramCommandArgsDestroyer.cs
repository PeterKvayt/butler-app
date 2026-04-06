using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Commands.ClassifyImage.Arguments;
using Api.Features.Telegram.Features.Command.Services.CommandArgument;
using Api.Infrastructure.FileSystem.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommandArgsDestroyer : ITelegramCommandArgsDestroyer
{
    private readonly IFileBufferService _fileBufferService;
    private readonly ICommandArgumentService _commandArgumentService;

    public ClassifyImageTelegramCommandArgsDestroyer(
        IFileBufferService fileSBufferService,
        ICommandArgumentService commandArgumentService
        )
    {
        _fileBufferService = fileSBufferService;
        _commandArgumentService = commandArgumentService;
    }

    public async ValueTask DestroyAsync()
    {
        var imageArg = _commandArgumentService.Get<ImageTelegramCommandArg>();

        if (imageArg.HasValue)
        {
            await _fileBufferService.DeleteFileAsync(imageArg.Value.Path);
        }
    }
}