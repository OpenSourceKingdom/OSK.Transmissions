using OSK.Transmissions.Messages.Abstractions;
using System;

namespace OSK.Transmissions.Abstractions
{
    public interface IMessageTransmissionContext
    {
        public IServiceProvider Services { get; }

        public object RawMessage { get; }

        public IMessage Message { get; }
    }
}
