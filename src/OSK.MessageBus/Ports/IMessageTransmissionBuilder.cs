using System;
using System.Collections.Generic;
using OSK.Hexagonal.MetaData;

namespace OSK.MessageBus.Ports
{
    /// <summary>
    /// Helps to construct a set of receivers that are tied to given transmitter for message transmission.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageTransmissionBuilder
    {
        /// <summary>
        /// The action used to configure message receivers
        /// </summary>
        /// <param name="configurator">The specific receiver configuration action</param>
        /// <returns>The transmission builder for chaining</returns>
        IMessageTransmissionBuilder AddConfigurator(Action<IMessageReceiverBuilder> configurator);

        /// <summary>
        /// Builds all receivers that have been configured with the builder
        /// </summary>
        /// <returns>The list of receivers attached to this builder for a given integration</returns>
        IEnumerable<IMessageReceiver> BuildReceivers();
    }
}
