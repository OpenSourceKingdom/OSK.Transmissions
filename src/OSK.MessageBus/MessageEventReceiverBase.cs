using Microsoft.Extensions.DependencyInjection;
using OSK.MessageBus.Events.Abstractions;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Models;
using OSK.MessageBus.Ports;
using System;
using System.Threading.Tasks;

namespace OSK.MessageBus
{
    public abstract class MessageEventReceiverBase(string receiverId, MessageEventTransmissionDelegate eventDelegate, IServiceProvider serviceProvider) 
        : IMessageEventReceiver
    {
        public string ReceiverId => receiverId;

        public abstract void Dispose();
        public abstract void Start();

        protected Task ProcessTransmissionAsync<T>(T message, object? rawMessageEvent)
            where T : IMessageEvent
        {
            using var scope = serviceProvider.CreateScope();
            var context = new MessageEventTransmissionContext<T>(scope.ServiceProvider, message, rawMessageEvent);
            return eventDelegate(context);
        }
    }
}
