using Microsoft.Extensions.Hosting;
using OSK.MessageBus.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Application
{
    public class MessageBusApplicationService(IMessageEventReceiverManager manager)
        : IHostedService
    {
        #region Variables

        private bool _configured;

        #endregion

        #region IHostedService

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(manager.Start);
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(manager.Stop);
        }

        #endregion
    }
}
