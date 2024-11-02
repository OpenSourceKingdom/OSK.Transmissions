using OSK.Hexagonal.MetaData;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Secondary)]
    public interface IMessageEventReceiverConfigurator
    {
        void ConfigureReceivers(IMessageEventReceiverManager manager);
    }
}
