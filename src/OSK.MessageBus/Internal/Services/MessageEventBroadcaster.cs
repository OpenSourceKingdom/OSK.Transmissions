using Microsoft.Extensions.DependencyInjection;
using OSK.Functions.Outputs.Abstractions;
using OSK.Functions.Outputs.Logging.Abstractions;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Internal.Services
{
    internal class MessageEventBroadcaster(IEnumerable<MessageEventTransmitterDescriptor> transmitterDescriptors,
        IServiceProvider serviceProvider,
        IOutputFactory<MessageEventBroadcaster> outputFactory)
        : IMessageEventBroadcaster
    {
        #region Variables

        private readonly static string OriginationSource = "OSK.MessageBus";

        #endregion

        #region IMessageEventBroadcaster

        public async Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(TMessage messageEvent, MessageBroadcastOptions options, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var targetedDescriptors = options.TransmitterTargetIds is null
                ? transmitterDescriptors
                : transmitterDescriptors.Where(transmitter => options.TransmitterTargetIds.Contains(transmitter.TransmitterId));

            var transmissionResults = new List<TransmissionResult>();
            foreach (var descriptor in targetedDescriptors)
            {
                var transmissionResult = new TransmissionResult()
                {
                    TransmitterId = descriptor.TransmitterId
                };

                var transmitter = (IMessageEventTransmitter) serviceProvider.GetRequiredService(descriptor.TransmitterType);
                try
                {
                    await transmitter.TransmitAsync(messageEvent, options.TransmissionOptions, cancellationToken);
                }
                catch (Exception ex)
                {
                    transmissionResult.Exception = ex;
                }

                transmissionResults.Add(transmissionResult);
            }

            var errorCount = transmissionResults.Count(transmission => transmission.Exception is not null);

            if (errorCount == 0)
            {
                return outputFactory.Success(new BroadcastResult()
                {
                    TransmissionResults = transmissionResults
                });
            }
            if (errorCount == transmissionResults.Count)
            {
                return outputFactory.Exception<BroadcastResult>
                    (new AggregateException(transmissionResults.Select(transmission => transmission.Exception)),
                    OriginationSource);
            }

            return outputFactory.Create(new BroadcastResult()
            {
                TransmissionResults = transmissionResults
            }, new OutputStatusCode(HttpStatusCode.MultiStatus, DetailCode.DownStreamError, OriginationSource));
        }

        #endregion
    }
}
