
using Api.Infrastructure.FileSystem.Features.Local.Extensions;
using Api.Infrastructure.FileSystem.Features.YandexDisk.Extensions;

namespace Api.Infrastructure.FileSystem;

internal static class Composition
{
    internal static WebApplicationBuilder AddLFileSystem(this WebApplicationBuilder builder)
    {
        builder.Services.AddLocalFileSystem();
        builder.Services.AddYandexDiskFileSystem(builder.Configuration);

        return builder;
    }
}