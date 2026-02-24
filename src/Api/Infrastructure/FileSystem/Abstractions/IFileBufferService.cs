namespace Api.Infrastructure.FileSystem.Abstractions;

internal interface IFileBufferService : 
    IFileSave, 
    IFileDelete,
    IFileGet
{
}
