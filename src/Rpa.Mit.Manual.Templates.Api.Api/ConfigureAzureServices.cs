using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

namespace Rpa.Mit.Manual.Templates.Api.Api
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureAzureServices
    {
        private const string Identifier = "RpaMitManualInvoice";

        public static IServiceCollection AddAzureBusComponents(
            this IServiceCollection services, WebApplicationBuilder builder)
            => services
            .AddAzureBusProcessor(builder)
            .AddAzureBusSender(builder);

        private static IServiceCollection AddAzureBusProcessor(this IServiceCollection services, WebApplicationBuilder builder)
            => services.AddSingleton(sp =>
            {
                var paymentHubOptions = builder.Configuration
                                           .GetSection("PAYMENTHUB")
                                           .Get<PaymentHub>();

                var client = new ServiceBusClient(paymentHubOptions?.CONNECTION);

                var _serviceBusProcessorOptions = new ServiceBusProcessorOptions()
                {
                    Identifier = $"{Identifier}-Reader"
                };

                return client.CreateProcessor(
                    paymentHubOptions?.RESPONSE_TOPIC,
                    paymentHubOptions?.RESPONSE_SUBSCRIPTION,
                    _serviceBusProcessorOptions);
            });

        private static IServiceCollection AddAzureBusSender(this IServiceCollection services, WebApplicationBuilder builder)
            => services.AddSingleton(sp =>
            {
                var paymentHubOptions = builder.Configuration
                       .GetSection("PAYMENTHUB")
                       .Get<PaymentHub>();

                var client = new ServiceBusClient(paymentHubOptions?.CONNECTION);

                return client.CreateSender(
                    paymentHubOptions?.TOPIC);
            });
    }
}
