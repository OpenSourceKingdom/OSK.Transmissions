using System;
using System.Collections.Generic;
using OSK.MessageBus.Ports;

namespace OSK.MessageBus.Internal.Services
{
    internal class MessageEventTransmissionBuilder<TReceiver>(IServiceProvider serviceProvider) : IMessageEventTransmissionBuilder<TReceiver>
        where TReceiver : IMessageEventReceiver
    {
        #region Variables

        private readonly List<Action<IMessageEventReceiverBuilder>> _transmissionConfigurators = [];
        private readonly Dictionary<string, IMessageEventReceiverBuilder> _receiverBuilders = [];

        #endregion

        #region IMessageEventTransmissionBuilder

        public IMessageEventTransmissionBuilder AddConfigurator(Action<IMessageEventReceiverBuilder> configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            _transmissionConfigurators.Add(configurator);
            return this;
        }

        public IMessageEventTransmissionBuilder<TReceiver> AddMessageEventReceiver(string receiverId, object[] parameters, 
            Action<IMessageEventReceiverBuilder> receiverBuilderConfiguration)
        {
            if (string.IsNullOrWhiteSpace(receiverId))
            {
                throw new ArgumentNullException(nameof(receiverId));
            }
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (receiverBuilderConfiguration is null)
            {
                throw new ArgumentNullException(nameof(receiverBuilderConfiguration));
            }
            if (_receiverBuilders.TryGetValue(receiverId, out _))
            {
                throw new InvalidOperationException($"Receiver id {receiverId} has already been added for receivers of type {typeof(TReceiver).FullName}");
            }

            var descriptor = new MessageEventReceiverDescriptor(receiverId, typeof(TReceiver), parameters);
            var builder = new MessageEventReceiverBuilder(serviceProvider, descriptor);
            receiverBuilderConfiguration(builder);

            _receiverBuilders.Add(receiverId, builder);
            return this;
        }

        public IEnumerable<IMessageEventReceiver> BuildReceivers()
        {
            foreach (var builder in _receiverBuilders.Values)
            {
                foreach (var configurator in _transmissionConfigurators)
                {
                    configurator.Invoke(builder);
                }

                yield return builder.BuildReceiver();
            }
        }

        #endregion
    }
}
