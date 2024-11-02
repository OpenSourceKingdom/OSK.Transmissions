using System;

namespace OSK.MessageBus.Events.Abstractions
{
    public interface ITenantedMessageEvent<TTenantId, TId>: IMessageEvent<TId>
        where TTenantId : struct, IEquatable<TTenantId>
        where TId : struct, IEquatable<TId>
    {
        TTenantId TenantId { get; } 
    }
}
