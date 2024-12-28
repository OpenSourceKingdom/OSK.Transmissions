using OSK.Functions.Outputs.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace OSK.MessageBus.Abstractions
{
    public static class MessageEventBroadcasterExtensions
    {
        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageEventBroadcaster broadcaster, TMessage messageEvent, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent
            => broadcaster.BroadcastMessageAsync(messageEvent, TimeSpan.Zero, cancellationToken);

        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageEventBroadcaster broadcaster, TMessage messageEvent, TimeSpan delayTimeSpan, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent 
            => broadcaster.BroadcastMessageAsync(messageEvent, options =>
            {
                options.TransmissionOptions = new MessageTransmissionOptions()
                {
                    DelayTimeSpan = delayTimeSpan
                };
            }, cancellationToken);

        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageEventBroadcaster broadcaster, TMessage messageEvent, 
            IEnumerable<string> transmitterTargetIds, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent
            => broadcaster.BroadcastMessageAsync(messageEvent, TimeSpan.Zero, transmitterTargetIds, cancellationToken);

        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageEventBroadcaster broadcaster, TMessage messageEvent, TimeSpan delayTimeSpan, 
            IEnumerable<string> transmitterTargetIds, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent
            => broadcaster.BroadcastMessageAsync(messageEvent, options =>
            {
                options.TransmissionOptions = new MessageTransmissionOptions()
                {
                    DelayTimeSpan = delayTimeSpan
                };
                options.TargetTransmitterIds = transmitterTargetIds.ToHashSet();
            }, cancellationToken);
    }
}
