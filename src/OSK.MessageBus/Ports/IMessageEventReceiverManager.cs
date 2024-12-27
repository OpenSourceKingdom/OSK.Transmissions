using OSK.Hexagonal.MetaData;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventReceiverManager: IDisposable
    {
        void Start();

        void Stop();
    }
}
