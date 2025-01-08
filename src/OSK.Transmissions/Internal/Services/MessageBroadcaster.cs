using Microsoft.Extensions.DependencyInjection;
using OSK.Functions.Outputs.Abstractions;
using OSK.Functions.Outputs.Logging.Abstractions;
using OSK.Transmissions.Abstractions;
using OSK.Transmissions.Internal;
using OSK.Transmissions.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.Transmissions.Internal.Services
{
    internal class MessageBroadcaster(IEnumerable<MessageTransmitterDescriptor> transmitterDescriptors,
        IServiceProvider serviceProvider,
        IOutputFactory<MessageBroadcaster> outputFactory)
        : IMessageBroadcaster
    {
        #region Variables

        private readonly static string OriginationSource = "OSK.Transmissions";

        #endregion

        #region IMessageBroadcaster

        public async Task<IOutputResponse<MessageTransmissionResult>> BroadcastMessageAsync<TMessage>(TMessage message, Action<MessageBroadcastOptions> broadcastConfiguration,
            CancellationToken cancellationToken = default)
            where TMessage : IMessage
        {
            if (broadcastConfiguration is null)
            {
                throw new ArgumentNullException(nameof(broadcastConfiguration));
            }

            MessageBroadcastOptions options = new();
            broadcastConfiguration(options);

            var targetedDescriptors = options.TargetTransmitterIds is null
                ? transmitterDescriptors
                : transmitterDescriptors.Where(transmitter => options.TargetTransmitterIds.Contains(transmitter.TransmitterId));

            var builder = outputFactory.BuildResponse<MessageTransmissionResult>();
            builder.WithOrigination(OriginationSource);

            foreach (var descriptor in targetedDescriptors)
            {
                var transmissionResult = new MessageTransmissionResult()
                {
                    TransmitterId = descriptor.TransmitterId
                };

                var transmitter = (IMessageTransmitter)serviceProvider.GetRequiredService(descriptor.TransmitterType);
                try
                {
                    await transmitter.TransmitAsync(message, options.TransmissionOptions, cancellationToken);
                    builder.AddSuccess(transmissionResult);
                }
                catch (Exception ex)
                {
                    builder.AddException(ex, transmissionResult);
                }
            }

            return builder.BuildResponse();
        }

        #endregion
    }
}
