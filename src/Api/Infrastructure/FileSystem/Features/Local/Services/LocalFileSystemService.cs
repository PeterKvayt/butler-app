using Api.Infrastructure.FileSystem.Abstractions;
using Api.Infrastructure.FileSystem.Extensions;

namespace Api.Infrastructure.FileSystem.Features.Local.Services;

internal sealed class LocalFileSystemService : IFileBufferService
{
    private readonly string _basePath;

    public LocalFileSystemService(IWebHostEnvironment webHostEnvironment)
    {
        _basePath = Path.Combine(webHostEnvironment.ContentRootPath, "tg", "files");
    }

    public async Task SaveFileAsync(Stream payload, string relativePath)
    {
        var absolutePath = GetAbsolutePath(relativePath);

        EnsurePathExists(absolutePath);

        await File.WriteAllBytesAsync(absolutePath, payload.GetBytes());
    }

    public async Task<Stream> GetFileAsync(string relativePath)
    {
        var absolutePath = GetAbsolutePath(relativePath);

        if (!File.Exists(absolutePath))
        {
            throw new InvalidOperationException($"There is no file: {absolutePath}");
        }

        var fileBytes = await File.ReadAllBytesAsync(absolutePath);

        return new MemoryStream(fileBytes);
    }

    public Task DeleteFileAsync(string relativePath)
    {
        var absolutePath = GetAbsolutePath(relativePath);

        File.Delete(absolutePath);

        return Task.CompletedTask;
    }

    private static void EnsurePathExists(string absolutePath)
    {
        var absoluteDirectoryPath = Path.GetDirectoryName(absolutePath);

        if (absoluteDirectoryPath != null && !Directory.Exists(absoluteDirectoryPath))
        {
            Directory.CreateDirectory(absoluteDirectoryPath);
        }
    }

    private string GetAbsolutePath(string relativePath) => Path.Combine(_basePath, relativePath);
}