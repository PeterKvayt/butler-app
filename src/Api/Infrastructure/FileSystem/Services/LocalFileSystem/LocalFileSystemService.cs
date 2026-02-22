using Api.Infrastructure.FileSystem.Abstractions;

namespace Api.Infrastructure.FileSystem.Services.LocalFileSystem;

internal sealed class LocalFileSystemService : IFileSystemService
{
    private readonly string _basePath;

    public LocalFileSystemService(IWebHostEnvironment webHostEnvironment)
    {
        _basePath = Path.Combine(webHostEnvironment.ContentRootPath, "tg", "files");
    }

    public Task SaveFileAsync(IReadOnlyCollection<byte> payload, string relativePath)
    {
        var path = Path.Combine(_basePath, relativePath);
        var directoryPath = Path.GetDirectoryName(path);

        if (directoryPath != null && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return File.WriteAllBytesAsync(path, payload.ToArray());
    }
}