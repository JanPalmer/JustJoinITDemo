using JustJoinITBackend.Common;
using JustJoinITBackend.Worker.Services;
using JustJoinITBackend.Worker.Services.ConnectorService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustJoinITBackend.Worker;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddDbContext<JustJoinITBackendDbContext>();

                services.AddSingleton<IConnectorService, ConnectorService>();

                services.AddHostedService<PromptProcessor>();
            })
            .Build();

        await host.RunAsync();
    }
}