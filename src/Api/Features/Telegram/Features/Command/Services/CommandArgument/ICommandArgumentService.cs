namespace Api.Features.Telegram.Features.Command.Services.CommandArgument
{
    internal interface ICommandArgumentService
    {
        T? Get<T>();
        T GetRequired<T>();
        void Set<T>(T value);
    }
}