using Microsoft.Extensions.DependencyInjection;

namespace OSK.MessageBus.Application
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a hosted service to the application that will start and stop message receivers in the background for a given application
        /// </summary>
        /// <param name="services">The dependency container's service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddHostedMessageBusManagement(this IServiceCollection services)
        {
            services.AddHostedService<MessageBusApplicationService>();

            return services;
        }
    }
}
