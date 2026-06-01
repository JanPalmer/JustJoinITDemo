using JustJoinITBackend.Worker.Services.Integrations;

namespace JustJoinITBackend.Worker.Fakes;

public class FakeModelConnectorFactory : IConnectorFactory
{
    private FakeModelConnector? _connector;

    public IConnector GetConnectorForModel(string modelName)
    {
        if (_connector == null)
        {
            _connector = new FakeModelConnector();
        }

        return _connector;
    }
}
