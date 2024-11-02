using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Models;
using System.Threading.Tasks;

namespace OSK.MessageBus.Ports
{
    public interface IMessageEventMiddleware
    {
        Task InvokeAsync(IMessageEventContext context, MessageEventDelegate next);
    }
}
