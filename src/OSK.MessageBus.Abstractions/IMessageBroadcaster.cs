using OSK.Functions.Outputs.Abstractions;
using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Events.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Abstractions
{
    /// <summary>
    /// Provides a consumer the means of broadcasting a transmission to a variety of backing message buses
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided, HexagonalIntegrationType.ConsumerPointOfEntry)]
    public interface IMessageBroadcaster
    {
        /// <summary>
        /// Broadcasts a message to message bus transmitters, provided with a set of broadcasting options
        /// </summary>
        /// <typeparam name="TMessage">The message type</typeparam>
        /// <param name="messageEvent">The message to broadcast</param>
        /// <param name="options">A set of configuration options used in the broadcasting</param>
        /// <param name="cancellationToken">The token to cancel the operation</param>
        /// <returns>An output of the broadcast result</returns>
        Task<IOutput<BroadcastResult>> BroadcastMessageAsync<TMessage>(TMessage messageEvent, Action<MessageBroadcastOptions> options, CancellationToken cancellationToken = default)
            where TMessage : IMessage;
    }
}
