using System;

namespace OSK.Transmissions.Exceptions
{
    public class MessageBusReceiverException : Exception
    {
        public MessageBusReceiverException(string message)
            : base(message)
        {
        }

        public MessageBusReceiverException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
