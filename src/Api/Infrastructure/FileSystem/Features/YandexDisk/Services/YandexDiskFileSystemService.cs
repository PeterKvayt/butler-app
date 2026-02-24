using Api.Infrastructure.FileSystem.Abstractions;
using System.Net;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Protocol;

namespace Api.Infrastructure.FileSystem.Features.YandexDisk.Services;

internal sealed class YandexDiskFileSystemService : IFileSystemService
{
    private readonly IDiskApi _diskApi;

    public YandexDiskFileSystemService(IDiskApi diskApi)
	{
        _diskApi = diskApi;
    }

    public async Task SaveFileAsync(Stream payload, string relativePath)
    {
        await EnsurePathExistedAsync(relativePath);

        await _diskApi.Files.UploadFileAsync(relativePath, overwrite: true, payload);
    }

    private async Task EnsurePathExistedAsync(string relativePath)
    {
        var directoryPath = Path.GetDirectoryName(relativePath).Replace('\\', '/');

        var folders = directoryPath.Trim('/').Split('/');

        if (folders.Length == 0)
        {
            return;
        }

        var path = string.Empty;

        foreach (var folder in folders)
        {
            path += $"/{folder}";
            try
            {
                var response = await _diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
                {
                    Path = path
                });
            }
            catch (YandexApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await _diskApi.Commands.CreateDictionaryAsync(path);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
