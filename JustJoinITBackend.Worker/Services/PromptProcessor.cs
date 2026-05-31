using JustJoinITBackend.Common;
using JustJoinITBackend.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustJoinITBackend.Worker.Services;

public class PromptProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly OllamaConnector _ollama;
    private readonly TimeSpan _pollingInterval;

    private const int DefaultPollingIntervalInSeconds = 5;

    public PromptProcessor(
        IServiceScopeFactory scopeFactory,
        OllamaConnector ollama,
        IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _ollama = ollama;

        var intervalFromConfig = config["Worker:PollingIntervalInSeconds"];
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

    private async Task ProcessNextPendingAsync(CancellationToken ct)
    {
        // Nowy scope na każdą iterację — DbContext jest Scoped, Worker jest Singleton
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<JustJoinITBackendDbContext>();

        // Pobierz najstarszy oczekujący prompt
        var prompt = await db.Prompts
            .Where(p => p.Status == DbPromptStatus.Pending)
            .OrderBy(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        if (prompt is null)
        {
            //_logger.LogDebug("Brak promptów do przetworzenia.");
            return;
        }

        await SetStatusAsync(db, prompt, DbPromptStatus.Processing, ct);

        try
        {
            var result = await _ollama.CompleteAsync(prompt.Content, ct);

            prompt.Result = result;
            await SetStatusAsync(db, prompt, DbPromptStatus.Completed, ct);
        }
        catch (Exception ex)
        {
            prompt.ErrorMessage = ex.Message;
            await SetStatusAsync(db, prompt, DbPromptStatus.Failed, ct);
        }
    }

    private static async Task SetStatusAsync(
        JustJoinITBackendDbContext db,
        DbPrompt prompt,
        DbPromptStatus status,
        CancellationToken ct)
    {
        prompt.Status = status;
        prompt.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
