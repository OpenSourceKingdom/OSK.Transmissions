using System;
using System.Collections.Generic;
using System.Text;
using OSK.MessageBus.Ports;

namespace OSK.MessageBus.Options
{
    public class MessageBusConfigurationOptions
    {
        #region Variables

        internal List<Action<IMessageEventReceiverBuilder>> GlobalReceiverBuilderConfiguration { get; private set; } = [];

        #endregion

        #region Helpers

        public MessageBusConfigurationOptions AddGlobalReceiverConfiguration(Action<IMessageEventReceiverBuilder> configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            GlobalReceiverBuilderConfiguration.Add(configuration);
            return this;
        }

        #endregion
    }
}
