using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Models;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventReceiverBuilder
    {
        IMessageEventReceiverBuilder Use(Func<MessageEventTransmissionDelegate, MessageEventTransmissionDelegate> middleware);

        IMessageEventReceiver BuildReceiver();
    }
}
