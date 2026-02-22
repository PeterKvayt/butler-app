
using Api.Infrastructure.FileSystem.Abstractions;
using Api.Infrastructure.FileSystem.Services.LocalFileSystem;

namespace Api.Infrastructure.FileSystem;

internal static class Composition
{
    internal static WebApplicationBuilder AddLFileSystem(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileSystemService, LocalFileSystemService>();
        
        return builder;
    }
}