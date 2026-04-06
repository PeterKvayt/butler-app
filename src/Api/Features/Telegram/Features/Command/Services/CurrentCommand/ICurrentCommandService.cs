
namespace Api.Features.Telegram.Features.Command.Services.CurrentCommand
{
    internal interface ICurrentCommandService
    {
        string? GetOrCreate(string? commandName);
        ValueTask RemoveAsync();
    }
}