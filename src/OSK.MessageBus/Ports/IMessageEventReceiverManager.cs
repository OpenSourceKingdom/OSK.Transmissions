using OSK.Hexagonal.MetaData;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageEventReceiverManager: IDisposable
    {
        void Start();

        void Stop();
    }
}
