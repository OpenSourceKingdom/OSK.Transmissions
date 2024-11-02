using OSK.MessageBus.Ports;

namespace OSK.MessageBus.Application
{
    public abstract class MessageBusReceiverConfigurationService
    {
        public abstract void ConfigureManager(IMessageEventReceiverManager manager);
    }
}
