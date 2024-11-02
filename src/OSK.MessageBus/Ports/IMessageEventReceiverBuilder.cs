using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Models;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventReceiverBuilder
    {
        IMessageEventReceiverBuilder Use(Func<MessageEventDelegate, MessageEventDelegate> middleware);

        IMessageEventReceiver BuildReceiver(string subscriptionId);
    }
}
