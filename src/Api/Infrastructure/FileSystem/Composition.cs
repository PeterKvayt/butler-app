
using Serilog;

namespace Api.Infrastructure.FileSystem;

internal static class Composition
{
    internal static WebApplicationBuilder AddLFileSystem(this WebApplicationBuilder builder)
    {
        builder.Services.AddScopedService<IFileSystemService, LocalFileSystemService>();
        
        return builder;
    }
}