using OSK.Functions.Outputs.Abstractions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using OSK.Transmissions.Messages.Abstractions;

namespace OSK.Transmissions.Abstractions
{
    public static class MessageBroadcasterExtensions
    {
        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageBroadcaster broadcaster, TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage
            => broadcaster.BroadcastMessageAsync(message, TimeSpan.Zero, cancellationToken);

        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageBroadcaster broadcaster, TMessage message, TimeSpan delayTimeSpan, CancellationToken cancellationToken = default)
            where TMessage : IMessage
            => broadcaster.BroadcastMessageAsync(message, options =>
            {
                options.TransmissionOptions = new MessageTransmissionOptions()
                {
                    DelayTimeSpan = delayTimeSpan
                };
            }, cancellationToken);

        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageBroadcaster broadcaster, TMessage message,
            IEnumerable<string> transmitterTargetIds, CancellationToken cancellationToken = default)
            where TMessage : IMessage
            => broadcaster.BroadcastMessageAsync(message, TimeSpan.Zero, transmitterTargetIds, cancellationToken);

        public static Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(this IMessageBroadcaster broadcaster, TMessage message, TimeSpan delayTimeSpan,
            IEnumerable<string> transmitterTargetIds, CancellationToken cancellationToken = default)
            where TMessage : IMessage
            => broadcaster.BroadcastMessageAsync(message, options =>
            {
                options.TransmissionOptions = new MessageTransmissionOptions()
                {
                    DelayTimeSpan = delayTimeSpan
                };
                options.TargetTransmitterIds = transmitterTargetIds.ToHashSet();
            }, cancellationToken);
    }
}
