using Microsoft.Extensions.Configuration;

namespace JustJoinITBackend.Worker.Services;

public class OllamaHttpClient : HttpClient
{
    public OllamaHttpClient(IConfiguration config)
    {
        var baseUrl = config["Ollama:BaseUrl"] ?? "http://localhost:11434";
        BaseAddress = new Uri(baseUrl);
    }
}
