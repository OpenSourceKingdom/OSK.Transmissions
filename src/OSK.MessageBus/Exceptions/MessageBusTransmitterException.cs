using System;

namespace OSK.MessageBus.Exceptions
{
    public class MessageBusTransmitterException: Exception
    {
        public MessageBusTransmitterException(string message)
            : base(message)
        {
        }

        public MessageBusTransmitterException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
