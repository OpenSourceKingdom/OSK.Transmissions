using System;

namespace OSK.MessageBus.Events.Abstractions
{
    public abstract class TenantedMessageBase<TTenantId, TId>: MessageEventBase<TId>, ITenantedMessage<TTenantId, TId>
        where TTenantId: struct, IEquatable<TTenantId>
        where TId: struct, IEquatable<TId>
    {
        public  TTenantId TenantId { get; set; }
    }
}
