using Microsoft.Extensions.Hosting;
using OSK.MessageBus.Ports;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.MessageBus.Application
{
    public class MessageBusApplicationService(IMessageEventReceiverManager manager,
        IEnumerable<MessageBusReceiverConfigurationService> receiverConfigurators)
        : IHostedService
    {
        #region Variables

        private bool _configured;

        #endregion

        #region IHostedService

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_configured)
            {
                foreach (var configurator in receiverConfigurators)
                {
                    configurator.ConfigureManager(manager);
                }
                _configured = true;
            }

            return Task.Run(manager.Start);
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(manager.Stop);
        }

        #endregion
    }
}
