using JustJoinITBackend.Worker.Services.Integrations;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace JustJoinITBackend.Worker.Services.Groq;

public class GroqConnector(IConfiguration config) : IConnector
{
    private readonly Dictionary<string, ChatClient> _clients = new();

    public void AddModel(string modelName)
    {
        if (!_clients.ContainsKey(modelName))
        {
            var apiKey = config["Models:Groq:APIKey"];
            var baseUrl = config["Models:Groq:BaseUrl"];
            if (apiKey == null || baseUrl == null)
            {
                // Log error - missing configuration
                return;
            }

            var endpoint = new Uri(baseUrl, UriKind.Absolute);
            var options = new OpenAIClientOptions { Endpoint = endpoint };
            _clients.Add(modelName, new ChatClient(modelName, new ApiKeyCredential(apiKey), options));
        }
    }

    public async Task<string?> SendPrompt(string modelName, string prompt, CancellationToken ct)
    {
        if (_clients.TryGetValue(modelName, out var chatClient))
        {
            var completion = await chatClient.CompleteChatAsync(
                                        [ChatMessage.CreateUserMessage(prompt)],
                                        cancellationToken: ct);

            if (completion == null || completion.Value.Content.Count == 0)
            {
                return null;
            }

            return completion.Value.Content[0].Text;
        }

        // Log error - model not found
        return null;
    }
}
