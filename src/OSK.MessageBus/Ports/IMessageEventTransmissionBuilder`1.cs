using System;
using OSK.Hexagonal.MetaData;

namespace OSK.MessageBus.Ports
{
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageEventTransmissionBuilder<TReceiver>: IMessageEventTransmissionBuilder
        where TReceiver: IMessageEventReceiver
    {
        IMessageEventTransmissionBuilder<TReceiver> AddMessageEventReceiver(string receiverId, object[] parameters, 
            Action<IMessageEventReceiverBuilder> receiverBuilder);

        IMessageEventTransmissionBuilder<TReceiver> AddMessageEventReceiver<TChildReceiver>(string receiverId, object[] parameters,
            Action<IMessageEventReceiverBuilder> receiverBuilder)
            where TChildReceiver: TReceiver;
    }
}
