using OSK.Hexagonal.MetaData;
using OSK.Transmissions.Messages.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.Transmissions.Abstractions
{
    /// <summary>
    /// The transmitter acts as the abstraction between the library and an underlying message bus implementation. This should be implemented per integration along with the needed receivers
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.IntegrationRequired)]
    public interface IMessageTransmitter
    {
        /// <summary>
        /// Transmits a message across the message bus
        /// </summary>
        /// <typeparam name="TMessage">The message type</typeparam>
        /// <param name="message">The message to transmit</param>
        /// <param name="options">Specific transmission options for the message bus to use</param>
        /// <param name="cancellationToken">The token to cancel the operation</param>
        /// <returns>The task completion object</returns>
        Task TransmitAsync<TMessage>(TMessage message, MessageTransmissionOptions options,
            CancellationToken cancellationToken = default)
            where TMessage : IMessage;
    }
}
