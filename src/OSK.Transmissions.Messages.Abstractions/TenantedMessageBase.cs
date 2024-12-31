using System;

namespace OSK.Transmissions.Messages.Abstractions
{
    public abstract class TenantedMessageBase<TTenantId, TId> : MessageBase<TId>, ITenantedMessage<TTenantId, TId>
        where TTenantId : struct, IEquatable<TTenantId>
        where TId : struct, IEquatable<TId>
    {
        public TTenantId TenantId { get; set; }
    }
}
