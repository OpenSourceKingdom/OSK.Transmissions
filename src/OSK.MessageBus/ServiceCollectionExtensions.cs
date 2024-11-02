using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Ports;

namespace OSK.MessageBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.TryAddTransient<IMessageEventReceiverManager, MessageEventReceiverManager>();
            services.TryAddTransient<IMessageEventSink, MessageEventSinkBase>();

            return services;
        }
    }
}
