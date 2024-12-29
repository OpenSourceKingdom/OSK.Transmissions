using Microsoft.Extensions.DependencyInjection;
using OSK.MessageBus.Models;
using OSK.MessageBus.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.MessageBus.Internal.Services
{
    internal class MessageReceiverBuilder(IServiceProvider serviceProvider, MessageReceiverDescriptor descriptor)
        : IMessageReceiverBuilder
    {
        #region Variables

        private const string UnhandledEventMessage = "No event handlers were registered and the event could not be processed.";

        private readonly List<Func<MessageTransmissionDelegate, MessageTransmissionDelegate>> _middlewares = [];

        #endregion

        #region IMessageEventReceiverBuilder

        public IMessageReceiverBuilder Use(Func<MessageTransmissionDelegate, MessageTransmissionDelegate> middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException("Middleware can not be null.", nameof(middleware));
            }

            _middlewares.Add(middleware);
            return this;
        }

        public IMessageReceiver BuildReceiver()
        {
            if (_middlewares.Count == 0)
            {
                throw new InvalidOperationException(UnhandledEventMessage);
            }

            MessageTransmissionDelegate messageTransmissionDelegate = static _ =>
            {
                throw new InvalidOperationException(UnhandledEventMessage);
            };
            messageTransmissionDelegate = Enumerable.Reverse(_middlewares)
                             .Aggregate(messageTransmissionDelegate, (aggregatedDelegate, middleware) => middleware(aggregatedDelegate));

            return (IMessageReceiver)ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ReceiverType,
                [descriptor.ReceiverId, messageTransmissionDelegate, ..descriptor.Parameters]);
        }

        #endregion
    }
}
