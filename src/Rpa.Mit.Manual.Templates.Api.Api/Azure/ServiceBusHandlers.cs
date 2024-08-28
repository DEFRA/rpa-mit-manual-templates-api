using Azure.Messaging.ServiceBus;

namespace Rpa.Mit.Manual.Templates.Api.Api.MitAzure
{
    public class ServiceBusHandlers
    {
        internal async Task PaymentHubResponseHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine("Received " + body);

            await args.CompleteMessageAsync(args.Message);
        }

        //private Task PaymentHubErrorHandler(ProcessErrorEventArgs arg)
        //{

        //}
    }
}
