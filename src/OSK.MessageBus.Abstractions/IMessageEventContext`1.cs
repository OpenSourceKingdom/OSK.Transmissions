using OSK.MessageBus.Events.Abstractions;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageEventContext<TMessage>: IMessageEventContext
        where TMessage : IMessageEvent
    {
        new TMessage Message { get; }
    }
}
