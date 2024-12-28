using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Models;
using System;

namespace OSK.MessageBus.Ports
{
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageEventReceiverBuilder
    {
        IMessageEventReceiverBuilder Use(Func<MessageEventTransmissionDelegate, MessageEventTransmissionDelegate> middleware);

        IMessageEventReceiver BuildReceiver();
    }
}
