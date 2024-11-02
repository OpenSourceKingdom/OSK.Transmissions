using System;

namespace OSK.MessageBus.Exceptions
{
    public class MessageBusPublishException: Exception
    {
        public MessageBusPublishException(string message)
            : base(message)
        {
        }

        public MessageBusPublishException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
