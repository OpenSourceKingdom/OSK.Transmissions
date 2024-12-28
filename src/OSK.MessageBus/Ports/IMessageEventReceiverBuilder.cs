using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Models;
using System;

namespace OSK.MessageBus.Ports
{
    /// <summary>
    /// Helps to construct a message event receiver for an integration and providing a means of customizing transmission processing
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageEventReceiverBuilder
    {
        /// <summary>
        /// Adds a middleware function that will act as an intermediate step during a chain of transmission processing
        /// </summary>
        /// <param name="middleware">The middleware function</param>
        /// <returns>The builder for chaining</returns>
        IMessageEventReceiverBuilder Use(Func<MessageEventTransmissionDelegate, MessageEventTransmissionDelegate> middleware);

        /// <summary>
        /// Builds the receiver object, with any current middleware being used in the transmission delegate
        /// </summary>
        /// <returns>The message receiver for a given integration</returns>
        IMessageEventReceiver BuildReceiver();
    }
}
