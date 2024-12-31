using System;
using System.Collections.Generic;
using OSK.Transmissions.Ports;

namespace OSK.Transmissions.Options
{
    public class MessageBusConfigurationOptions
    {
        #region Variables

        internal List<Action<IMessageReceiverBuilder>> GlobalReceiverBuilderConfiguration { get; private set; } = [];

        #endregion

        #region Helpers

        public MessageBusConfigurationOptions AddGlobalReceiverConfiguration(Action<IMessageReceiverBuilder> configuration)
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
