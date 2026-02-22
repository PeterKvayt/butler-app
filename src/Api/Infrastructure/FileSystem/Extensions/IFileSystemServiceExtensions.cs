
using Api.Infrastructure.FileSystem.Abstractions;

namespace Api.Infrastructure.FileSystem.Extensions;

internal static class IFileSystemServiceExtensions
{
    internal static Task SaveFileAsync(this IFileSystemService fileSystemService, Stream payload, string relativePath)
    {
        payload.Seek(0, SeekOrigin.Begin);
        var bytesPayload = GetBytes(payload);
        return fileSystemService.SaveFileAsync(bytesPayload, relativePath);
    }

    private static byte[] GetBytes(Stream stream)
    {
        using var binaryReader = new BinaryReader(stream);

        return binaryReader.ReadBytes((int)stream.Length);
    }
}
