using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Events.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Abstractions
{
    [HexagonalIntegration(HexagonalIntegrationType.IntegrationRequired)]
    public interface IMessageEventTransmitter
    {
        Task TransmitAsync<TMessage>(TMessage message, MessageTransmissionOptions options,
            CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent;
    }
}
