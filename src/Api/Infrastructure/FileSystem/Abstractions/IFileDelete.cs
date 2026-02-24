namespace Api.Infrastructure.FileSystem.Abstractions;

internal interface IFileDelete
{
    internal Task DeleteFileAsync(string relativePath);
}
