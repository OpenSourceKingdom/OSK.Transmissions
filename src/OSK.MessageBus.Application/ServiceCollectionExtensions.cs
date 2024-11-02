using Microsoft.Extensions.DependencyInjection;

namespace OSK.MessageBus.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHostedMessageBusManagement(this IServiceCollection services)
        {
            services.AddHostedService<MessageBusApplicationService>();

            return services;
        }
    }
}
