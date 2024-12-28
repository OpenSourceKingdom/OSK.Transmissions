using Microsoft.Extensions.DependencyInjection;
using OSK.MessageBus.Models;
using OSK.MessageBus.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.MessageBus.Internal.Services
{
    internal class MessageEventReceiverBuilder(IServiceProvider serviceProvider, MessageEventReceiverDescriptor descriptor)
        : IMessageEventReceiverBuilder
    {
        #region Variables

        private const string UnhandledEventMessage = "No event handlers were registered and the event could not be processed.";

        private readonly List<Func<MessageEventTransmissionDelegate, MessageEventTransmissionDelegate>> _middlewares = [];

        #endregion

        #region IMessageEventReceiverBuilder

        public IMessageEventReceiverBuilder Use(Func<MessageEventTransmissionDelegate, MessageEventTransmissionDelegate> middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException("Middleware can not be null.", nameof(middleware));
            }

            _middlewares.Add(middleware);
            return this;
        }

        public IMessageEventReceiver BuildReceiver()
        {
            if (_middlewares.Count == 0)
            {
                throw new InvalidOperationException(UnhandledEventMessage);
            }

            MessageEventTransmissionDelegate eventDelegate = static _ =>
            {
                throw new InvalidOperationException(UnhandledEventMessage);
            };
            eventDelegate = Enumerable.Reverse(_middlewares)
                             .Aggregate(eventDelegate, (aggregatedDelegate, middleware) => middleware(aggregatedDelegate));

            return (IMessageEventReceiver)ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ReceiverType,
                [descriptor.ReceiverId, eventDelegate, ..descriptor.Parameters]);
        }

        #endregion
    }
}
