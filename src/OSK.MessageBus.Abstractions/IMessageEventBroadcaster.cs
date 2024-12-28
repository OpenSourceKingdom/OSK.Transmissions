using OSK.Functions.Outputs.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Abstractions
{
    public interface IMessageEventBroadcaster
    {
        Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(TMessage messageEvent, Action<MessageBroadcastOptions> options, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent;
    }
}
