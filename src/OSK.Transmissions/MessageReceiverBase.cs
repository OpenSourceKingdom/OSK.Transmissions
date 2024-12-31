using Microsoft.Extensions.DependencyInjection;
using OSK.Transmissions.Internal.Services;
using OSK.Transmissions.Messages.Abstractions;
using OSK.Transmissions.Models;
using OSK.Transmissions.Ports;
using System;
using System.Threading.Tasks;

namespace OSK.Transmissions
{
    public abstract class MessageReceiverBase(string receiverId, MessageTransmissionDelegate transmissionDelegate, IServiceProvider serviceProvider)
        : IMessageReceiver
    {
        public string ReceiverId => receiverId;

        public abstract void Dispose();
        public abstract void Start();

        protected Task ProcessTransmissionAsync<T>(T message, object? rawMessage)
            where T : IMessage
        {
            using var scope = serviceProvider.CreateScope();
            var context = new MessageTransmissionContext<T>(scope.ServiceProvider, message, rawMessage);
            return transmissionDelegate(context);
        }
    }
}
