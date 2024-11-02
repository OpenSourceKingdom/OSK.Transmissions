using OSK.Functions.Outputs.Abstractions;
using OSK.Functions.Outputs.Logging.Abstractions;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using OSK.MessageBus.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus
{
    public abstract class MessageEventSinkBase(IMessageEventPublisher publisher,
        IOutputFactory<MessageEventSinkBase> outputFactory, string orginationSource)
        : IMessageEventSink
    {
        #region IMessageEventSink

        public async Task<IOutput> PublishAsync<TMessage>(TMessage messageEvent, MessagePublishOptions options, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent
        {
            try
            {
                await publisher.PublishAsync(messageEvent, options, cancellationToken);
                return outputFactory.Success();
            }
            catch (Exception ex)
            {
                return outputFactory.Exception(ex, orginationSource);
            }
        }

        #endregion
    }
}
