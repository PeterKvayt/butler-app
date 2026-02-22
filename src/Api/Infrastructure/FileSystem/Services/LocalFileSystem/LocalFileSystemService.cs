internal sealed class LocalFileSystemService : IFileSystemService
{
    private readonly string _basePath;

    internal LocalFileSystemService(WebHostEnvironment webHostEnvironment)
    {
        _basePath = Path.Combine(webHostEnvironment.ContentRootPath, "telegram", "files");
    }

    public Task SaveFileAsync(IReadOnlyCollection<byte> payload, string relativePath)
    {
        var path = Path.Combine(_basePath, relativePath);
        return File.WriteAllBytesAsync(path, payload);
    }
}