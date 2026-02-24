namespace Api.Infrastructure.FileSystem.Extensions;

internal static class StreamExtensions
{
    internal static byte[] GetBytes(this Stream stream)
    {
        using var binaryReader = new BinaryReader(stream);

        return binaryReader.ReadBytes((int)stream.Length);
    }
}
