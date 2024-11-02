using OSK.MessageBus.Abstractions;
using System.Threading.Tasks;

namespace OSK.MessageBus.Ports
{
    public interface IMessageEventHandler
    {
        Task HandleEventAsync(IMessageEventContext context);
    }
}
