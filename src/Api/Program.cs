using Api.Infrastructure;
using Api.Infrastructure.Telegram;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApp();

var app = builder
    .Build()
    .ConfigurePipeline();

await app.ConfigureTelegramWebhookAsync();

app.Run();