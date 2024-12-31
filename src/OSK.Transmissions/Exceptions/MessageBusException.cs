using System;

namespace OSK.Transmissions.Exceptions
{
    public class MessageBusException : Exception
    {
        public MessageBusException(string message)
            : base(message)
        {
        }

        public MessageBusException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
