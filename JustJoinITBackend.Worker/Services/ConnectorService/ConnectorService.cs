using JustJoinITBackend.Common;
using JustJoinITBackend.Worker.Fakes;
using JustJoinITBackend.Worker.Services.Integrations;
using JustJoinITBackend.Worker.Services.Integrations.Gemini;
using JustJoinITBackend.Worker.Services.Integrations.Groq;
using Microsoft.Extensions.Configuration;

namespace JustJoinITBackend.Worker.Services.ConnectorService;

public class ConnectorService : IConnectorService
{
    private readonly Dictionary<string, IConnector> _models;

    public ConnectorService(JustJoinITBackendDbContext dbContext, IConfiguration configuration)
    {
        var connectorFactories = new Dictionary<string, IConnectorFactory>()
        {
            { "FakeModel", new FakeModelConnectorFactory() },
            { "Gemini", new GeminiConnectorFactory(configuration) },
            { "Groq", new GroqConnectorFactory(configuration) },
        };

        _models = new Dictionary<string, IConnector>();

        var models = dbContext.Models.ToList();

        foreach (var model in models)
        {
            try
            {
                if (connectorFactories.TryGetValue(model.Family, out var factory))
                {
                    _models.Add(model.Name, factory.GetConnectorForModel(model.Name));
                }
                else
                {
                    // Log missing factory for the model family
                }
            }
            catch (Exception ex)
            {
                // Log the error for the specific model
            }
        }
    }

    public IConnector? GetModelConnector(string modelName)
    {
        if (_models.TryGetValue(modelName, out var connector))
        {
            return connector;
        }

        return null;
    }
}
