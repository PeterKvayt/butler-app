using Api.Infrastructure.FileSystem.Abstractions;
using Api.Infrastructure.FileSystem.Features.YandexDisk.Options;
using Api.Infrastructure.FileSystem.Features.YandexDisk.Services;
using Microsoft.Extensions.Options;
using YandexDisk.Client;
using YandexDisk.Client.Http;

namespace Api.Infrastructure.FileSystem.Features.YandexDisk.Extensions;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddYandexDiskFileSystem(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<YandexDiskOptions>(configuration.GetSection("Yandex:Disk"))
            .AddScoped<IFileSystemService, YandexDiskFileSystemService>();

        services.AddYandexDiskClient();

        return services;
    }

    private static void AddYandexDiskClient(this IServiceCollection services)
    {
        services.AddSingleton<IDiskApi>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<YandexDiskOptions>>();

            return new DiskHttpApi(options.Value.AccessToken);
        });
    }
}
