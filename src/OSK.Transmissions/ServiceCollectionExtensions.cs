using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSK.Transmissions.Internal;
using OSK.Transmissions.Abstractions;
using OSK.Transmissions.Internal.Services;
using OSK.Transmissions.Options;
using OSK.Transmissions.Ports;

namespace OSK.Transmissions
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
            services.TryAddTransient(typeof(IMessageReceiverGroupBuilder<>), typeof(MessageReceiverGroupBuilder<>));

            services.Configure(configuration);

            return services;
        }

        #endregion

        #region Helpers

        public static IServiceCollection AddMessageTransmitter<TTransmitter, TReceiver>(this IServiceCollection services, string transmitterId,
            Action<IMessageReceiverGroupBuilder<TReceiver>> receiverGroupConfiguration)
            where TTransmitter : IMessageTransmitter
            where TReceiver : IMessageReceiver
        {
            if (receiverGroupConfiguration is null)
            {
                throw new ArgumentNullException(nameof(receiverGroupConfiguration));
            }

            services.AddTransient(_ => new MessageTransmitterDescriptor(transmitterId, typeof(TTransmitter)));
            services.AddTransient(serviceProvider =>
            {
                var receiverGroupBuilder = serviceProvider.GetRequiredService<IMessageReceiverGroupBuilder<TReceiver>>();
                receiverGroupConfiguration(receiverGroupBuilder);

                return receiverGroupBuilder;
            });

            return services;
        }

        #endregion
    }
}
