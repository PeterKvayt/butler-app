using Api.Features.Telegram.Features.Command.Abstractions;
using Api.Features.Telegram.Features.Command.Helpers;
using Api.Features.Telegram.Features.Command.Models;
namespace Api.Features.Telegram.Features.Command.Providers.TelegramCommandArgsBuilder;

internal sealed class TelegramCommandArgsBuilderProvider : ITelegramCommandArgsBuilderProvider
{
    private readonly IServiceProvider _serviceProvider;

    public TelegramCommandArgsBuilderProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITelegramCommandArgsBuilder GetBuilder(string commandName)
    {
        var key = TelegramCommandArgsBuilderKeyHelper.CreateKey(commandName);
        return _serviceProvider.GetRequiredKeyedService<ITelegramCommandArgsBuilder>(key);
    }

    public ITelegramCommandArgsBuilder GetBuilder(CommandContextModel commandContextModel)
    {
        var builder = GetBuilder(commandContextModel.CommandName);
        builder.Arguments = commandContextModel.CommandArgs;
        return builder;
    }
}
