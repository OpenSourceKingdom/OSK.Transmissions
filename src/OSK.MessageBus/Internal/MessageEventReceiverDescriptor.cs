using System;

namespace OSK.MessageBus.Internal
{
    internal class MessageEventReceiverDescriptor(string receiverId, Type receiverType, object[] parameters)
    {
        public string ReceiverId => receiverId;

        public Type ReceiverType => receiverType;

        public object[] Parameters => parameters;
    }
}
