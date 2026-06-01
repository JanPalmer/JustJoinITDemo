using JustJoinITBackend.Worker.Services.Groq;
using Microsoft.Extensions.Configuration;

namespace JustJoinITBackend.Worker.Services.Integrations.Groq;

public class GroqConnectorFactory(IConfiguration configuration) : IConnectorFactory
{
    private GroqConnector? _connector;

    public IConnector GetConnectorForModel(string modelName)
    {
        if (_connector == null)
        {
            _connector = new GroqConnector(configuration);
        }

        _connector.AddModel(modelName);

        return _connector;
    }
}
