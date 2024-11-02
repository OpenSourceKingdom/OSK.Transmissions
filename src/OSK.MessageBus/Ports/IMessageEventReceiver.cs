using OSK.Hexagonal.MetaData;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventReceiver: IDisposable
    {
        void Start();
    }
}
