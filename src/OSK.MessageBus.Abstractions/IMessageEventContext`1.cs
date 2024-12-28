using OSK.MessageBus.Events.Abstractions;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageEventContext<TMessage>: IMessageTransmissionContext
        where TMessage : IMessageEvent
    {
        new TMessage Message { get; }
    }
}
