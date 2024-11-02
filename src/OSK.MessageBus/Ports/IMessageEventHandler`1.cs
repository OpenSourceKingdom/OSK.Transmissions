using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System.Threading.Tasks;

namespace OSK.MessageBus.Ports
{
    public interface IMessageEventHandler<TMessage>
        where TMessage: IMessageEvent
    {
        Task HandleEventAsync(IMessageEventContext<TMessage> context);
    }
}
