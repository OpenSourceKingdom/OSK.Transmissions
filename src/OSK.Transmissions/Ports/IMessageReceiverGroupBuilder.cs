﻿using System;
using System.Collections.Generic;
using OSK.Hexagonal.MetaData;

namespace OSK.Transmissions.Ports
{
    /// <summary>
    /// Helps to construct a set of receivers that are tied to given transmitter for message transmission.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageReceiverGroupBuilder
    {
        /// <summary>
        /// The action used to configure message receivers
        /// </summary>
        /// <param name="configurator">The specific receiver configuration action</param>
        /// <returns>The group builder for chaining</returns>
        IMessageReceiverGroupBuilder AddConfigurator(Action<IMessageReceiverBuilder> configurator);

        /// <summary>
        /// Builds all receivers that have been configured with the builder
        /// </summary>
        /// <returns>The list of receivers attached to this builder for a given integration</returns>
        IEnumerable<IMessageReceiver> BuildReceivers();
    }
}
