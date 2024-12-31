using OSK.Transmissions.Messages.Abstractions;

namespace OSK.Transmissions.Abstractions
{
    public interface IMessageTransmissionContext<TMessage> : IMessageTransmissionContext
        where TMessage : IMessage
    {
        new TMessage Message { get; }
    }
}
