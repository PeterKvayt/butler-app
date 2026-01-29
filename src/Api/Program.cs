using Api.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApp();

var app = builder
    .Build()
    .ConfigurePipeline();

app.Run();