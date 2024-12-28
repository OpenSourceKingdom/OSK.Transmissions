using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using OSK.MessageBus.Models;
using System.Threading.Tasks;

namespace OSK.MessageBus.Ports
{
    public interface IMessageEventMiddleware<TEvent>
        where TEvent: IMessageEvent
    {
        Task InvokeAsync(IMessageEventContext<TEvent> context, MessageEventTransmissionDelegate next);
    }
}
