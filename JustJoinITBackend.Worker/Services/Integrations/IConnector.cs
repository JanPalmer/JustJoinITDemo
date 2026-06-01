namespace JustJoinITBackend.Worker.Services.Integrations;

public interface IConnector
{
    Task<string?> SendPrompt(string modelName, string prompt, CancellationToken cancellationToken);
}
