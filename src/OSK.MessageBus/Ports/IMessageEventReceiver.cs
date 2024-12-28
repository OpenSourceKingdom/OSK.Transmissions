using OSK.Hexagonal.MetaData;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalIntegration(HexagonalIntegrationType.IntegrationRequired)]
    public interface IMessageEventReceiver: IDisposable
    {
        void Start();
    }
}
