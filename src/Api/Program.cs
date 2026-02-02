using Api.Features.Telegram;
using Api.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApp();

var app = builder
    .Build()
    .ConfigurePipeline();

await app.SetupTelegramAsync();

await app.RunAsync();