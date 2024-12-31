using Microsoft.Extensions.Hosting;
using OSK.Transmissions.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.Transmissions.Application
{
    public class MessageBusApplicationService(IMessageReceiverManager manager)
        : IHostedService
    {
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
