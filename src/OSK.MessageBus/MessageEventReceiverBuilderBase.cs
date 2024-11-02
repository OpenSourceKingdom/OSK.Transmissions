using OSK.MessageBus.Models;
using OSK.MessageBus.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.MessageBus
{
    public abstract class MessageEventReceiverBuilderBase : IMessageEventReceiverBuilder
    {
        #region Variables

        private const string UnhandledEventMessage = "No event handlers were registered and the event could not be processed.";

        private readonly List<Func<MessageEventDelegate, MessageEventDelegate>> _middlewares = new();

        #endregion

        #region IMessageEventReceiverBuilder

        public IMessageEventReceiverBuilder Use(Func<MessageEventDelegate, MessageEventDelegate> middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException("Middleware can not be null.", nameof(middleware));
            }

            _middlewares.Add(middleware);
            return this;
        }

        public IMessageEventReceiver BuildReceiver(string subscriptionId)
        {
            return BuildReceiver(subscriptionId, BuildMessageEventDelegate());
        }

        #endregion

        #region Helpers

        protected abstract IMessageEventReceiver BuildReceiver(string subscriptionId, MessageEventDelegate eventDelegate);

        private MessageEventDelegate BuildMessageEventDelegate()
        {
            if (_middlewares.Count == 0)
            {
                throw new InvalidOperationException(UnhandledEventMessage);
            }

            MessageEventDelegate eventDelegate = static _ =>
            {
                throw new InvalidOperationException(UnhandledEventMessage);
            };
            return Enumerable.Reverse(_middlewares)
                             .Aggregate(eventDelegate, (aggregatedDelegate, middleware) => middleware(aggregatedDelegate));
        }

        #endregion
    }
}
