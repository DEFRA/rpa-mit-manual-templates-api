namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    /// <summary>
    /// sends back to the business the responses we get from the payment hub.
    /// </summary>
    public class BusinessFeedbackService
    {
        private readonly ILogger<BusinessFeedbackService> _logger;

        public BusinessFeedbackService(ILogger<BusinessFeedbackService> logger)
        {
            _logger = logger;
        }

        public async Task SendPaymentHubResponses()
        {
            await Task.Delay(100);
            _logger.LogInformation(
                "Sample Service did something.");
        }
    }
}
