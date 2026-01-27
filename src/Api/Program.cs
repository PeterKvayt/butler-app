using Api.Infrastructure;
using Api.Infrastructure.TelegramBot;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApp();

var app = builder
    .Build()
    .ConfigurePipeline();

await app.ConfigureTelegramWebhookAsync();

app.Run();