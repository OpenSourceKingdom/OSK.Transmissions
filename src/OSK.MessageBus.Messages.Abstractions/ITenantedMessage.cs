using System;

namespace OSK.MessageBus.Messages.Abstractions
{
    /// <summary>
    /// Represents a typed message that is associated with a typed identifier and a typed parent identifier
    /// </summary>
    /// <typeparam name="TTenantId">The tenant, or parent, identifier type</typeparam>
    /// <typeparam name="TId">The message identifier type</typeparam>
    public interface ITenantedMessage<TTenantId, TId> : IMessage<TId>
        where TTenantId : struct, IEquatable<TTenantId>
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// The message's tenant identifier
        /// </summary>
        TTenantId TenantId { get; }
    }
}
