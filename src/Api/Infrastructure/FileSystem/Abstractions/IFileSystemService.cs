namespace Api.Infrastructure.FileSystem.Abstractions;

internal interface IFileSystemService
{
    internal Task SaveFileAsync(IReadOnlyCollection<byte> payload, string relativePath);
}