using System;

namespace OSK.MessageBus.Events.Abstractions
{
    /// <summary>
    /// Represents a typed message that is associated with a typed identifier and a typed parent identifier
    /// </summary>
    /// <typeparam name="TTenantId">The tenant, or parent, identifier type</typeparam>
    /// <typeparam name="TId">The message identifier type</typeparam>
    public interface ITenantedMessageEvent<TTenantId, TId>: IMessageEvent<TId>
        where TTenantId : struct, IEquatable<TTenantId>
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// The message's tenant identifier
        /// </summary>
        TTenantId TenantId { get; } 
    }
}
