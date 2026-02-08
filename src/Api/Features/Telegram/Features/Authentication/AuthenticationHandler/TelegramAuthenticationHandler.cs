using Api.Features.Telegram.Features.Authentication.Constants;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Authentication.AuthenticationHandler;
internal sealed class TelegramAuthenticationHandler(
    JsonSerializerOptions jsonSerializerOptions, 
    ITelegramBotClient telegramBotClient) 
    : IAuthenticationHandler
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    private HttpContext Context { get; set; } = default!;
    private AuthenticationScheme Scheme { get; set; } = default!;

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var update = await Context.Request.ReadFromJsonAsync<Update>(_jsonSerializerOptions);

        Context.Items[HttpContextItemKeys.TelegramIncomingUpdate] = update;

        if (update == null)
        {
            return AuthenticateResult.NoResult();
        }

        var user = GetUser(update);
        if (user == null)
        {
            return AuthenticateResult.NoResult();
        }

        var claims = CreateClaims(user);
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var authenticationTicket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(authenticationTicket);
    }

    public Task ChallengeAsync(AuthenticationProperties? properties)
    {
        Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }

    public async Task ForbidAsync(AuthenticationProperties? properties)
    {
        Context.Response.StatusCode = StatusCodes.Status200OK;

        if (Context.Items.TryGetValue(HttpContextItemKeys.TelegramIncomingUpdate, out var rawUpdate)
            && rawUpdate is Update update && update.Message?.Chat != null)
        {
            await _telegramBotClient
                .SendMessage(update.Message.Chat.Id, "Sorry, you don't have access to the bot");
        }
    }

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        Context = context;
        Scheme = scheme;

        return Task.CompletedTask;
    }

    private static User? GetUser(Update update) => update.Type switch
    {
        UpdateType.Message => update.Message?.From,
        UpdateType.Unknown => null,
        UpdateType.InlineQuery => update.InlineQuery?.From,
        UpdateType.ChosenInlineResult => update.ChosenInlineResult?.From,
        UpdateType.CallbackQuery => update.CallbackQuery?.From,
        UpdateType.EditedMessage => update.EditedMessage?.From,
        UpdateType.ChannelPost => update.ChannelPost?.From,
        UpdateType.EditedChannelPost => update.EditedChannelPost?.From,
        UpdateType.ShippingQuery => update.ShippingQuery?.From,
        UpdateType.PreCheckoutQuery => update.PreCheckoutQuery?.From,
        UpdateType.Poll => null,
        UpdateType.PollAnswer => update.PollAnswer?.User,
        UpdateType.MyChatMember => update.MyChatMember?.From,
        UpdateType.ChatMember => update.ChatMember?.From,
        UpdateType.ChatJoinRequest => update.ChatJoinRequest?.From,
        UpdateType.MessageReaction => update.MessageReaction?.User,
        UpdateType.MessageReactionCount => null,
        UpdateType.ChatBoost => null,
        UpdateType.RemovedChatBoost => null,
        UpdateType.BusinessConnection => update.BusinessConnection?.User,
        UpdateType.BusinessMessage => update.BusinessMessage?.From,
        UpdateType.EditedBusinessMessage => update.EditedBusinessMessage?.From,
        UpdateType.DeletedBusinessMessages => null,
        UpdateType.PurchasedPaidMedia => update.PurchasedPaidMedia?.From,
        _ => null
    };

    private static List<Claim> CreateClaims(User user)
    {
        var claims = new List<Claim>(4)
        {
            new(TelegramClaims.Id, user.Id.ToString(), ClaimValueTypes.Integer64),
        };

        if (user.Username != null)
        {
            claims.Add(new(TelegramClaims.Username, user.Username, ClaimValueTypes.String));
        }

        return claims;
    }
}
