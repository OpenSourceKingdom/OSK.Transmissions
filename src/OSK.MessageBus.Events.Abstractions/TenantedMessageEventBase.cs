using System;

namespace OSK.MessageBus.Events.Abstractions
{
    public abstract class TenantedMessageEventBase<TTenantId, TId>: MessageEventBase<TId>, ITenantedMessageEvent<TTenantId, TId>
        where TTenantId: struct, IEquatable<TTenantId>
        where TId: struct, IEquatable<TId>
    {
        public  TTenantId TenantId { get; set; }
    }
}
