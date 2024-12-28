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
        #region Add Core Services

        /// <summary>
        /// Adds the necessary services t
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMessageTransmissions(this IServiceCollection services)
            => services.AddMessageTransmissions(_ => { });

        public static IServiceCollection AddMessageTransmissions(this IServiceCollection services,
            Action<MessageBusConfigurationOptions> configuration)
        {
            services.TryAddTransient<IMessageEventReceiverManager, MessageEventReceiverManager>();
            services.TryAddTransient<IMessageEventBroadcaster, MessageEventBroadcaster>();
            services.TryAddTransient(typeof(IMessageEventTransmissionBuilder<>), typeof(MessageEventTransmissionBuilder<>));

            services.Configure(configuration);

            return services;
        }

        #endregion

        #region Helpers

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

        #endregion
    }
}
