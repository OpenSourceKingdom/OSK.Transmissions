using OSK.MessageBus.Events.Abstractions;
using System;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageEventContext
    {
        public IServiceProvider Services { get; }

        public object RawMessage { get; }

        public IMessageEvent Message { get; }
    }
}
