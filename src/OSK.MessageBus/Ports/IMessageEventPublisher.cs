using OSK.Hexagonal.MetaData;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventPublisher
    {
        Task PublishAsync<TMessage>(TMessage message, MessagePublishOptions options, 
            CancellationToken cancellationToken = default)
            where TMessage: IMessageEvent;
    }
}
