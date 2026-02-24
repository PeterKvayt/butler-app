namespace Api.Infrastructure.FileSystem.Abstractions;

internal interface IFileSave
{
    internal Task SaveFileAsync(Stream payload, string relativePath);
}
