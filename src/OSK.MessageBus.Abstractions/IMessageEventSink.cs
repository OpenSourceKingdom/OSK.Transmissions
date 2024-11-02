using OSK.Functions.Outputs.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageEventSink
    {
        Task<IOutput> PublishAsync<TMessage>(TMessage messageEvent, MessagePublishOptions options, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent;
    }
}
