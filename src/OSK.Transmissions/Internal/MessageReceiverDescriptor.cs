using System;

namespace OSK.Transmissions.Internal
{
    internal class MessageReceiverDescriptor(string receiverId, Type receiverType, object[] parameters)
    {
        public string ReceiverId => receiverId;

        public Type ReceiverType => receiverType;

        public object[] Parameters => parameters;
    }
}
