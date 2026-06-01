using Microsoft.Extensions.Configuration;

namespace JustJoinITBackend.Worker.Services.Integrations.Gemini;

public class GeminiConnectorFactory(IConfiguration configuration) : IConnectorFactory
{
    private GeminiConnector? _connector;

    public IConnector GetConnectorForModel(string modelName)
    {
        if (_connector == null)
        {
            _connector = new GeminiConnector(configuration);
        }

        return _connector;
    }
}
