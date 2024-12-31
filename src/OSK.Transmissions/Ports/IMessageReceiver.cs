using OSK.Hexagonal.MetaData;
using System;

namespace OSK.Transmissions.Ports
{
    /// <summary>
    /// Represents an object that listens for message transmissions from a <see cref="Abstractions.IMessageTransmitter"/>. Integrations should
    /// implement this along with the respective transmitter.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.IntegrationRequired)]
    public interface IMessageReceiver : IDisposable
    {
        /// <summary>
        /// Starts the active listening state for the receiver
        /// </summary>
        void Start();
    }
}
