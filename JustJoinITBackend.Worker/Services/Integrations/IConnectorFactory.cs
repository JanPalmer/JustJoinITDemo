namespace JustJoinITBackend.Worker.Services.Integrations;

public interface IConnectorFactory
{
    IConnector GetConnectorForModel(string modelName);
}
