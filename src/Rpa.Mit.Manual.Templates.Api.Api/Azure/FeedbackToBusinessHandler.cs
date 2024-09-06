using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    /// <summary>
    /// sends back to the business the responses we get from the payment hub.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BusinessFeedbackHandler
    {
        private readonly ILogger<BusinessFeedbackHandler> _logger;
        private readonly IEmailService _iEmailService;

        public BusinessFeedbackHandler(
            ILogger<BusinessFeedbackHandler> logger, 
            IEmailService iEmailService)
        {
            _logger = logger;
            _iEmailService = iEmailService;
        }

        public async Task SendPaymentHubResponses(CancellationToken cancelToken)
        {
            PaymentHubResponseRoot message = new PaymentHubResponseRoot();
            message!.paymentRequest = new PaymentHubResponsePaymentRequest();

            message.error = "HUIO HUIO HUIO";
            message!.paymentRequest!.InvoiceRequestId = "JHOP jhiop gyui ftyi";

            await _iEmailService.EmailPaymentHubError("aylmer.carson.external@eviden.com", message, cancelToken);

            _logger.LogInformation(
                "Sample Service did something.");
        }
    }
}
