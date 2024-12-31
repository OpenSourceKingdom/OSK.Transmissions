using OSK.Hexagonal.MetaData;
using OSK.Transmissions.Abstractions;
using OSK.Transmissions.Messages.Abstractions;
using OSK.Transmissions.Models;
using System.Threading.Tasks;

namespace OSK.Transmissions.Ports
{
    /// <summary>
    /// Represents an intermediate step in the chain of message processing. It is expected that a message will be continued by invoking the delegate.
    /// This allows chain of logic to be run on a message transmission as it moves through the delegate.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.ConsumerOptional, HexagonalIntegrationType.IntegrationOptional)]
    public interface IMessageTransmissionMiddleware<TMessage>
        where TMessage : IMessage
    {
        /// <summary>
        /// Processes the message at an intermediate step. This method should call the delegate that is passed for the transmission chain of processing to comlpete.
        /// </summary>
        /// <param name="context">The message contextual data as it arrived from a transmitter</param>
        /// <param name="next">The delegate used to continue the chain of calls for the message transmission</param>
        /// <returns>The task completion object</returns>
        Task InvokeAsync(IMessageTransmissionContext<TMessage> context, MessageTransmissionDelegate next);
    }
}
