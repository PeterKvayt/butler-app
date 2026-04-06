namespace Api.Features.Telegram.Features.Command.Services.CommandArgument
{
    internal interface ICommandArgumentService
    {
        T? Get<T>() where T: struct;
        T GetRequired<T>() where T : struct;
        void Set<T>(T value);
        ValueTask ClearArgumentsAsync(string commandName);
    }
}