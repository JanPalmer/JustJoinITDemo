using JustJoinITBackend.Worker.Services.Integrations;

namespace JustJoinITBackend.Worker.Fakes;

public class FakeModelConnector : IConnector
{
    public Task<string?> SendPrompt(string modelName, string prompt, CancellationToken ct)
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return Task.FromResult($"Test response for {modelName}: {prompt}");
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }
}
