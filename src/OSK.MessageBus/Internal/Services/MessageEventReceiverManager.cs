using OSK.MessageBus.Ports;
using System;
using System.Collections.Generic;

namespace OSK.MessageBus.Internal.Services
{
    internal class MessageEventReceiverManager(IServiceProvider serviceProvider) : IMessageEventReceiverManager
    {
        #region Variables

        private readonly Dictionary<string, Func<IServiceProvider, IEnumerable<Action<IMessageEventReceiverBuilder>>, IMessageEventReceiverBuilder>> _builderFactories = new();
        private readonly List<Action<IMessageEventReceiverBuilder>> _globalConfigurators = new();
        private List<IMessageEventReceiver> _receivers = new();
        private bool _started;

        #endregion

        #region IMessageEventReceiverManager

        public IMessageEventReceiverManager AddConfigurator(Action<IMessageEventReceiverBuilder> configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            _globalConfigurators.Add(configurator);
            return this;
        }

        public IMessageEventReceiverManager AddEventReceiver(string subscriptionId,
            Func<IServiceProvider, IEnumerable<Action<IMessageEventReceiverBuilder>>, IMessageEventReceiverBuilder> factory)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentException("Subscription id can not be empty.", nameof(subscriptionId));
            }
            if (_builderFactories.TryGetValue(subscriptionId, out _))
            {
                throw new InvalidOperationException($"Subcription id, {subscriptionId}, has already been added.");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("Factory can not be null.", nameof(factory));
            }

            _builderFactories[subscriptionId] = factory;
            return this;
        }

        public void Start()
        {
            if (_started)
            {
                return;
            }

            _receivers = new List<IMessageEventReceiver>();
            foreach (var buildFactory in _builderFactories)
            {
                var eventReceiverBuilder = buildFactory.Value(serviceProvider, _globalConfigurators);
                var receiver = eventReceiverBuilder.BuildReceiver(buildFactory.Key);
                receiver.Start();

                _receivers.Add(receiver);
            }

            _started = true;
        }

        public void Stop()
        {
            if (!_started)
            {
                return;
            }

            var exceptions = new List<Exception>();
            foreach (var receiver in _receivers)
            {
                try
                {
                    receiver.Dispose();
                }
                catch(Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            _receivers.Clear();
            _started = false;

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}
