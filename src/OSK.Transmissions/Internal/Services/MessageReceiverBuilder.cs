using Microsoft.Extensions.DependencyInjection;
using OSK.Transmissions.Internal;
using OSK.Transmissions.Models;
using OSK.Transmissions.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Transmissions.Internal.Services
{
    internal class MessageReceiverBuilder(IServiceProvider serviceProvider, MessageReceiverDescriptor descriptor)
        : IMessageReceiverBuilder
    {
        #region Variables

        private const string UnhandledMessageError = "No message handlers were registered and the message could not be processed.";

        private readonly List<Func<MessageTransmissionDelegate, MessageTransmissionDelegate>> _middlewares = [];

        #endregion

        #region IMessageReceiverBuilder

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
                throw new InvalidOperationException(UnhandledMessageError);
            }

            MessageTransmissionDelegate messageTransmissionDelegate = static _ =>
            {
                throw new InvalidOperationException(UnhandledMessageError);
            };
            messageTransmissionDelegate = Enumerable.Reverse(_middlewares)
                             .Aggregate(messageTransmissionDelegate, (aggregatedDelegate, middleware) => middleware(aggregatedDelegate));

            return (IMessageReceiver)ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ReceiverType,
                [descriptor.ReceiverId, messageTransmissionDelegate, .. descriptor.Parameters]);
        }

        #endregion
    }
}
