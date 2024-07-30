using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.MitAzure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureAzureServices
    {
        public static IServiceCollection ConfigureAzure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventQueueService>(_ => new EventQueueService(new ServiceBusProvider(configuration), configuration));

            services.AddTransient<IServiceBusProvider, ServiceBusProvider>();

            return services;
        }
    }
}
