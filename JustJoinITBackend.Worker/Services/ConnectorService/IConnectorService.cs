using JustJoinITBackend.Worker.Services.Integrations;

namespace JustJoinITBackend.Worker.Services.ConnectorService;

public interface IConnectorService
{
    IConnector? GetModelConnector(string modelName);
}
