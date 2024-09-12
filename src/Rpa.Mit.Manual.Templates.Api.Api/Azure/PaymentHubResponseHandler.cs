using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    /// <summary>
    /// handles responses from the payment hub servicebus topic
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class PaymentHubResponseHandler : IMessageHandler<PaymentHubResponseRoot>
    {
        private readonly ILogger<PaymentHubResponseHandler> _logger;
        private readonly IEmailService _iEmailService;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly IInvoiceRepo _iInvoiceRepo;

        public PaymentHubResponseHandler(
            IEmailService iEmailService,    
            IInvoiceRepo iInvoiceRepo,  
            IInvoiceRequestRepo iInvoiceRequestRepo,
            ILogger<PaymentHubResponseHandler> logger)
        {
            _iEmailService = iEmailService;
            _logger = logger;
            _iInvoiceRepo = iInvoiceRepo;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        /// <summary>
        /// ought to be looking into the outbox pattern for this, 
        /// but time constraints and restrictions on what i can do on azure...
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Task> HandleAsync(PaymentHubResponseRoot message, CancellationToken cancelToken)
        {
            try
            {
                var paymentHubResponseForDatabase = new PaymentHubResponseForDatabase
                {
                    invoicenumber = Guid.Parse(message!.paymentRequest!.invoiceNumber),
                    invoicerequestid = message!.paymentRequest!.InvoiceRequestId,
                    paymenthubdateprocessed = DateTime.UtcNow,
                    error = message.error,
                    accepted = message.accepted
                };

                // update the database...
                await _iInvoiceRequestRepo.UpdateInvoiceRequestWithPaymentHubResponse(paymentHubResponseForDatabase);

                //if we have an error, we also need to email the originator of the data with the relevant data.
                if (!message.accepted)
                {
                    // first get the relevant user email
                    var emailAddress = await _iInvoiceRepo.GetInvoiceCreatorEmailAddress(paymentHubResponseForDatabase.invoicenumber, cancelToken);

                    //var html = GetMyTable<PaymentHubResponsePaymentRequest>(message.paymentRequest, x => x.InvoiceRequestId, x => x.frn, x => x.value);

                    await _iEmailService.EmailPaymentHubError(emailAddress, message, cancelToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // throw this so it it caught by the processor
                throw new Exception(ex.Message);
            }

            return Task.CompletedTask;
        }

        public static string GetMyTable<T>(T item, params Func<T, object>[] fxns)
        {
            // Dynamic build  HTML Table Header column name base on T\model object
            Type type = item!.GetType();
            PropertyInfo[] props = type.GetProperties();
            string THstr = "<tr>";
            foreach (var prop in props)
            {
                THstr += "<TH>" + prop.Name + "</TH>";
            }
            THstr += "</tr>";

            // Build  remain data rows in HTML Table
            StringBuilder sb = new StringBuilder();
            // Inject bootstrap class base on your need
            sb.Append("<TABLE class='table table-sm table-dark'>\n");
            sb.Append(THstr);

            sb.Append("<TR>\n");
            foreach (var fxn in fxns)
            {
                sb.Append("<TD>");
                sb.Append(fxn(item));
                sb.Append("</TD>");
            }
            sb.Append("</TR>\n");

            sb.Append("</TABLE>");

            return sb.ToString();
        }

        //public static string GetMyTable<T>(T item, params Func<T, object>[] fxns)
        //{

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<TABLE>\n");

        //    sb.Append("<TR>\n");
        //    foreach (var fxn in fxns)
        //    {
        //        sb.Append("<TD>");
        //        sb.Append(fxn(item));
        //        sb.Append("</TD>");
        //    }
        //    sb.Append("</TR>\n");

        //    sb.Append("</TABLE>");

        //    return sb.ToString();
        //}
    }
}
