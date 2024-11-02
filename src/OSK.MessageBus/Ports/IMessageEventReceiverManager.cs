using OSK.Hexagonal.MetaData;
using System;
using System.Collections.Generic;

namespace OSK.MessageBus.Ports
{
    [HexagonalPort(HexagonalPort.Primary)]
    public interface IMessageEventReceiverManager: IDisposable
    {
        IMessageEventReceiverManager AddConfigurator(Action<IMessageEventReceiverBuilder> configurator);

        IMessageEventReceiverManager AddEventReceiver(string subscriptionId,
            Func<IServiceProvider, IEnumerable<Action<IMessageEventReceiverBuilder>>, IMessageEventReceiverBuilder> factory);

        void Start();

        void Stop();
    }
}
