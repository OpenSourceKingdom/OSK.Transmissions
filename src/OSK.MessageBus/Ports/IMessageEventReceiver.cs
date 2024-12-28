using OSK.Hexagonal.MetaData;
using System;

namespace OSK.MessageBus.Ports
{
    /// <summary>
    /// Represents an object that listens for message transmissions from a <see cref="Abstractions.IMessageEventTransmitter"/>. Integrations should
    /// implement this along with the respective transmitter.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.IntegrationRequired)]
    public interface IMessageEventReceiver: IDisposable
    {
        /// <summary>
        /// Starts the active listening state for the receiver
        /// </summary>
        void Start();
    }
}
