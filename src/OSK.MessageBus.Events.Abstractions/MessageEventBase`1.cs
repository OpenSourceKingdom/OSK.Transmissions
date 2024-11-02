using System;

namespace OSK.MessageBus.Events.Abstractions
{
    public abstract class MessageEventBase<TId>: MessageEventBase, IMessageEvent<TId>
        where TId : struct, IEquatable<TId>
    {
        public TId Id { get; set; }
    }
}
