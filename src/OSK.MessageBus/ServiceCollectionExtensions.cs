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
            services.TryAddTransient<IMessageReceiverManager, MessageReceiverManager>();
            services.TryAddTransient<IMessageBroadcaster, MessageBroadcaster>();
            services.TryAddTransient(typeof(IMessageTransmissionBuilder<>), typeof(MessageTransmissionBuilder<>));

            services.Configure(configuration);

            return services;
        }

        #endregion

        #region Helpers

        public static IServiceCollection AddMessageEventTransmitter<TTransmitter, TReceiver>(this IServiceCollection services, string transmitterId,
            Action<IMessageTransmissionBuilder<TReceiver>> transmissionBuilderConfiguration)
            where TTransmitter : IMessageTransmitter
            where TReceiver : IMessageReceiver
        {
            if (transmissionBuilderConfiguration is null)
            {
                throw new ArgumentNullException(nameof(transmissionBuilderConfiguration));
            }

            services.AddTransient(_ => new MessageTransmitterDescriptor(transmitterId, typeof(TTransmitter)));
            services.AddTransient(serviceProvider =>
            {
                var transmissionBuilder = serviceProvider.GetRequiredService<IMessageTransmissionBuilder<TReceiver>>();
                transmissionBuilderConfiguration(transmissionBuilder);

                return transmissionBuilder;
            });

            return services;
        }

        #endregion
    }
}
