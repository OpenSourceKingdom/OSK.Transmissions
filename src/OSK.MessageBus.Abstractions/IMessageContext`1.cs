using OSK.MessageBus.Messages.Abstractions;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageTransmissionContext<TMessage>: IMessageTransmissionContext
        where TMessage : IMessage
    {
        new TMessage Message { get; }
    }
}
