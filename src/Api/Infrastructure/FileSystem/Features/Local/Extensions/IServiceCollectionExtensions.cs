using Api.Infrastructure.FileSystem.Abstractions;
using Api.Infrastructure.FileSystem.Features.Local.Services;

namespace Api.Infrastructure.FileSystem.Features.Local.Extensions;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddLocalFileSystem(this IServiceCollection services)
    {
        services.AddScoped<IFileBufferService, LocalFileSystemService>();

        return services;
    }
}
