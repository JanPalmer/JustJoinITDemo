using JustJoinITBackend.Common;
using JustJoinITBackend.Common.Models;
using JustJoinITBackend.Worker.Services.ConnectorService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustJoinITBackend.Worker.Services;

public class PromptProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnectorService _connectorService;

    private readonly TimeSpan _pollingInterval;

    private const int DefaultPollingIntervalInSeconds = 5;
    private const string ModelAPIConnectionErrorMessage = "Error connecting to the model. Please try again later.";

    public PromptProcessor(
        IServiceScopeFactory scopeFactory,
        IConfiguration config,
        IConnectorService connectorService
        )
    {
        _scopeFactory = scopeFactory;
        _connectorService = connectorService;

        var intervalFromConfig = config["WorkerSettings:PollingIntervalInSeconds"];
        var interval = int.TryParse(intervalFromConfig, out var seconds) ? seconds : DefaultPollingIntervalInSeconds;

        _pollingInterval = TimeSpan.FromSeconds(interval);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ProcessNextPendingAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Normalne zamknięcie — nie logujemy jako błąd
                break;
            }
            catch (Exception ex)
            {
                // Błąd poza przetwarzaniem konkretnego promptu (np. problem z DB)
            }

            await Task.Delay(_pollingInterval, cancellationToken);
        }
    }

    private async Task ProcessNextPendingAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JustJoinITBackendDbContext>();

        var prompt = await dbContext.Prompts
            .Include(p => p.Model)
            .Where(p => p.Status == DbPromptStatus.Pending)
            .OrderBy(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        if (prompt == null)
        {
            return;
        }

        await UpdatePrompt(
            dbContext,
            prompt,
            DbPromptStatus.Processing,
            cancellationToken: cancellationToken);

        try
        {
            var modelConnector = _connectorService.GetModelConnector(prompt.Model.Name);

            if (modelConnector == null)
            {
                throw new InvalidOperationException($"Model not found - {prompt.Model.Name}");
            }

            var result = await modelConnector.SendPrompt(prompt.Model.Name, prompt.Content, cancellationToken);

            if (result != null)
            {
                await UpdatePrompt(
                    dbContext,
                    prompt,
                    DbPromptStatus.Completed,
                    result: result,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await UpdatePrompt(
                    dbContext,
                    prompt,
                    DbPromptStatus.Failed,
                    errorMessage: ModelAPIConnectionErrorMessage,
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            // Log exception
            await UpdatePrompt(
                dbContext,
                prompt,
                DbPromptStatus.Failed,
                errorMessage: ModelAPIConnectionErrorMessage,
                cancellationToken: cancellationToken);
        }
    }

    private static async Task UpdatePrompt(
        JustJoinITBackendDbContext dbContext,
        DbPrompt prompt,
        DbPromptStatus status,
        string? result = null,
        string? errorMessage = null,
        CancellationToken cancellationToken = default)
    {
        prompt.Status = status;
        prompt.UpdatedAt = DateTime.UtcNow;

        if (result != null)
        {
            prompt.Result = result;
        }

        if (errorMessage != null)
        {
            prompt.ErrorMessage = errorMessage;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
