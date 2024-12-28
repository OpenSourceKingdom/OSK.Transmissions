using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System.Threading.Tasks;

namespace OSK.MessageBus.Ports
{

    /// <summary>
    /// Represents the end of the chain of transmission middleware and completion of the message processing. Once a message has arrived at a handler,
    /// it is expected that all message processing will be completed here.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.ConsumerOptional, HexagonalIntegrationType.IntegrationOptional)]
    public interface IMessageTransmissionHandler<TMessage>
        where TMessage: IMessage
    {
        /// <summary>
        /// Handles the final message processing of a message transmission
        /// </summary>
        /// <param name="context">The message contextual data as it arrived from a transmitter</param>
        /// <returns>The task completion object</returns>
        Task HandleEventAsync(IMessageTransmissionContext<TMessage> context);
    }
}
