namespace Api.Infrastructure.FileSystem.Abstractions;

internal interface IFileGet
{
    internal Task<Stream> GetFileAsync(string relativePath);
}
