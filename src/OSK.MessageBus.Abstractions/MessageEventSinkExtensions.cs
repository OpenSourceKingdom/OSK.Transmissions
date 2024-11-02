using OSK.Functions.Outputs.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace OSK.MessageBus.Abstractions
{
    public static class MessageEventSinkExtensions
    {
        public static Task<IOutput> PublishAsync<TMessage>(this IMessageEventSink eventSink, TMessage messageEvent, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent => eventSink.PublishAsync(messageEvent, TimeSpan.Zero, cancellationToken);

        public static Task<IOutput> PublishAsync<TMessage>(this IMessageEventSink eventSink, TMessage messageEvent, TimeSpan delayTimeSpan, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent => eventSink.PublishAsync(messageEvent, new MessagePublishOptions()
            {
                DelayTimeSpan = delayTimeSpan
            }, cancellationToken);
    }
}
