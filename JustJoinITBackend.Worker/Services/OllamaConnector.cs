using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace JustJoinITBackend.Worker.Services;

public class OllamaConnector
{
    private readonly OllamaHttpClient _http;
    private readonly string _model;

    public OllamaConnector(
        OllamaHttpClient http,
        IConfiguration config)
    {
        _http = http;
        _model = config["Ollama:Model"] ?? "llama3.2";
    }

    public async Task<string> CompleteAsync(string prompt, CancellationToken ct)
    {
        var request = new OllamaRequest
        {
            Model = _model,
            Prompt = prompt,
            Stream = false
        };

        //var response = await _http.PostAsJsonAsync("/api/generate", request, ct);
        //response.EnsureSuccessStatusCode();

        //var result = await response.Content
        //    .ReadFromJsonAsync<OllamaResponse>(ct);

        //if (result?.Response is null)
        //    throw new InvalidOperationException(
        //        "Ollama zwróciła pustą odpowiedź.");

        //return result.Response;

        return "Response - Ok";
    }

    // ── DTOs ─────────────────────────────────────────────────────────────────

    private sealed class OllamaRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false;
    }

    private sealed class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string? Response { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }

        [JsonPropertyName("total_duration")]
        public long? TotalDuration { get; set; }
    }
}
