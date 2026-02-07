using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Api.Features.Telegram.Features.Authentication.Handlers;

internal sealed class TelegramAuthenticationHandler(JsonSerializerOptions jsonSerializerOptions) : IAuthenticationHandler
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

    private HttpContext Context { get; set; } = default!;
    private AuthenticationScheme Scheme { get; set; } = default!;

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        if (Context.Request.ContentType != "application/json"
            || !Context.Request.Path.StartsWithSegments($"/{UpdateHandler.Endpoint.Segment}"))
        {
            return AuthenticateResult.NoResult();
        }

        Context.Request.EnableBuffering();

        var update = await Context.Request.ReadFromJsonAsync<Update>(_jsonSerializerOptions);
        Context.Request.Body.Seek(0, SeekOrigin.Begin);

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
        Context.Response.StatusCode = 401;
        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties? properties)
    {
        Context.Response.StatusCode = 403;
        return Task.CompletedTask;
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
            new(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer64),
            new(ClaimTypes.Name, user.FirstName, ClaimValueTypes.String),
        };

        if (user.Username != null)
        {
            claims.Add(new("TelegramUsername", user.Username, ClaimValueTypes.String));
        }

        if (user.LastName != null)
        {
            claims.Add(new(ClaimTypes.Surname, user.LastName, ClaimValueTypes.String));
        }

        return claims;
    }
}
