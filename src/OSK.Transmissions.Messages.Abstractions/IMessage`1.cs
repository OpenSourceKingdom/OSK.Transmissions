using System;

namespace OSK.Transmissions.Messages.Abstractions
{
    /// <summary>
    /// Represents a message that has a typed identifier for an application
    /// </summary>
    /// <typeparam name="TId">The message identifier type</typeparam>
    public interface IMessage<TId> : IMessage
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// The message identifier
        /// </summary>
        public TId Id { get; }
    }
}
