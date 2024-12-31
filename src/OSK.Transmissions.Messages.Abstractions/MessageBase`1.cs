using System;

namespace OSK.Transmissions.Messages.Abstractions
{
    public abstract class MessageBase<TId> : MessageBase, IMessage<TId>
        where TId : struct, IEquatable<TId>
    {
        public TId Id { get; set; }
    }
}
