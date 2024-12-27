using System;
using OSK.Hexagonal.MetaData;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventTransmissionBuilder<TReceiver>: IMessageEventTransmissionBuilder
        where TReceiver: IMessageEventReceiver
    {
        IMessageEventTransmissionBuilder<TReceiver> AddMessageEventReceiver(string receiverId, object[] parameters, 
            Action<IMessageEventReceiverBuilder> receiverBuilder);
    }
}
