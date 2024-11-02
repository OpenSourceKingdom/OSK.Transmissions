using System;

namespace OSK.MessageBus.Events.Abstractions
{
    public interface IMessageEvent<TId>: IMessageEvent
        where TId : struct, IEquatable<TId>
    {
        public TId Id { get; }
    }
}
