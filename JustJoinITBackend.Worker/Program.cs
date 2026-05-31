using JustJoinITBackend.Common;
using JustJoinITBackend.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<JustJoinITBackendDbContext>();

        services.AddScoped<OllamaHttpClient>();
        services.AddScoped<OllamaConnector>();

        services.AddHostedService<PromptProcessor>();
    })
    .Build();

await host.RunAsync();
