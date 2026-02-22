using Api.Features.Telegram.Features.Command.Abstractions;
using Telegram.Bot.Types;

namespace Api.Features.Telegram.Features.Command.Commands.UploadFile;

internal sealed class UploadFileTelegramCommandArgsBuilder : ITelegramCommandArgsBuilder
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IFileSystemService _fileSystemservice;

    private UploadFileTelegramCommandArgs _args = new();

    private string _nextArgMessage = string.Empty;

    public ITelegramCommandArgs Arguments
    {
        get => _args;
        set {
            if (value is not UploadFileTelegramCommandArgs args)
            {
                throw new InvalidOperationException($"Unsupported args of type {value.GetType()}");
            }

            _args = args;
        }
    }
    
    internal UploadFileTelegramCommandArgsBuilder(ITelegramBotClient telegramBotClient, IFileSystemService fileSystemservice)
    {
        _telegramBotClient = telegramBotClient;
        _fileSystemservice = fileSystemservice;
    }

    public void AddAgrument(Message message)
    {
        if (!_args.ChatId.HasValue)
        {
            _args.ChatId = message.Chat.Id;
        }

        if (string.IsNullEmptyOrWhitespace(_args.TempFilePath))
        {
            if () // if message does not have a photo
            {
                _nextArgMessage = "Upload the photo";
                return;
            }

            _fileSystemservice.SaveFileAsync()
            // save photo 
            // set path to args
        }
    }

    public Task RequestNextAgrumentAsync() => _telegramBotClient.SendMessage(_args.GetChatId(), _nextArgMessage);

    public bool IsArgumentsFilledIn()
    {
        return _args.ChatId.HasValue && !_string.IsNullEmptyOrWhitespace(_args.TempFilePath);
    }
}
