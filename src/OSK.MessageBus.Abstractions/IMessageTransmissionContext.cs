using OSK.MessageBus.Events.Abstractions;
using System;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageTransmissionContext
    {
        public IServiceProvider Services { get; }

        public object RawMessage { get; }

        public IMessage Message { get; }
    }
}
