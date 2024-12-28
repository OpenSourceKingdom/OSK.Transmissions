using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Internal;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Options;
using OSK.MessageBus.Ports;

namespace OSK.MessageBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
            => services.AddMessageBus(_ => { });

        public static IServiceCollection AddMessageBus(this IServiceCollection services, 
            Action<MessageBusConfigurationOptions> configuration)
        {
            services.TryAddTransient<IMessageEventReceiverManager, MessageEventReceiverManager>();
            services.TryAddTransient<IMessageEventBroadcaster, MessageEventBroadcaster>();
            services.TryAddTransient(typeof(IMessageEventTransmissionBuilder<>), typeof(MessageEventTransmissionBuilder<>));

            services.Configure(configuration);

            return services;
        }

        public static IServiceCollection AddMessageEventTransmitter<TTransmitter, TReceiver>(this IServiceCollection services, string transmitterId,
            Action<IMessageEventTransmissionBuilder<TReceiver>> transmissionBuilderConfiguration)
            where TTransmitter : IMessageEventTransmitter
            where TReceiver : IMessageEventReceiver
        {
            if (transmissionBuilderConfiguration is null)
            {
                throw new ArgumentNullException(nameof(transmissionBuilderConfiguration));
            }

            services.AddTransient(_ => new MessageEventTransmitterDescriptor(transmitterId, typeof(TTransmitter)));
            services.AddTransient(serviceProvider =>
            {
                var transmissionBuilder = serviceProvider.GetRequiredService<IMessageEventTransmissionBuilder<TReceiver>>();
                transmissionBuilderConfiguration(transmissionBuilder);

                return transmissionBuilder;
            });

            return services;
        }
    }
}
