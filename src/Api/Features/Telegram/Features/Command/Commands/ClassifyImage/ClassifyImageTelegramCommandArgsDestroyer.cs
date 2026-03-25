using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Infrastructure.FileSystem.Abstractions;

namespace Api.Features.Telegram.Features.Command.Commands.ClassifyImage;

internal sealed class ClassifyImageTelegramCommandArgsDestroyer : ITelegramCommandArgsDestroyer
{
    private readonly IFileBufferService _fileBufferService;

    public ClassifyImageTelegramCommandArgsDestroyer(IFileBufferService fileSBufferService)
    {
        _fileBufferService = fileSBufferService;
    }

    public async ValueTask DestroyAsync(ITelegramCommandArgs arguments)
    {
        if (arguments is not ClassifyImageTelegramCommandArgs args)
        {
            throw new InvalidOperationException($"{arguments.GetType().Name} is not supported");
        }

        var tasks = new List<Task>();

        AddTask(args.ImagePath, tasks);

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