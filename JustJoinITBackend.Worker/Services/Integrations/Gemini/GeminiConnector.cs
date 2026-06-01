using Google.GenAI;
using Microsoft.Extensions.Configuration;

namespace JustJoinITBackend.Worker.Services.Integrations.Gemini;

public class GeminiConnector : IConnector
{
    private readonly Client _client;

    public GeminiConnector(
        IConfiguration config)
    {
        var key = config["Models:Gemini:APIKey"]; // Log error if key is null or empty

        _client = new Client(apiKey: key);
    }

    public async Task<string?> SendPrompt(string modelName, string prompt, CancellationToken ct)
    {
        var response = await _client.Models.GenerateContentAsync(
                                  model: modelName,
                                  contents: prompt,
                                  cancellationToken: ct
                                );

        if (response == null)
        {
            return null;
        }

        return response.Text;
    }
}
