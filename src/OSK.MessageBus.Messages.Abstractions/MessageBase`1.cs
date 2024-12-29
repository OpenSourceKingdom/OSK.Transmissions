using System;

namespace OSK.MessageBus.Messages.Abstractions
{
    public abstract class MessageEventBase<TId> : MessageBase, IMessage<TId>
        where TId : struct, IEquatable<TId>
    {
        public TId Id { get; set; }
    }
}
