using System;
using System.Collections.Generic;
using OSK.Hexagonal.MetaData;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventTransmissionBuilder
    {
        IMessageEventTransmissionBuilder AddConfigurator(Action<IMessageEventReceiverBuilder> configurator);

        IEnumerable<IMessageEventReceiver> BuildReceivers();
    }
}
