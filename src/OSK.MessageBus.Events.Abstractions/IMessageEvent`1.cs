using System;

namespace OSK.MessageBus.Events.Abstractions
{
    /// <summary>
    /// Represents a message that has a typed identifier for an application
    /// </summary>
    /// <typeparam name="TId">The message identifier type</typeparam>
    public interface IMessageEvent<TId>: IMessageEvent
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// The message identifier
        /// </summary>
        public TId Id { get; }
    }
}
