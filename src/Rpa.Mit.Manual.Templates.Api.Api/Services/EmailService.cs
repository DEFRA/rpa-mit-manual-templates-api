using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

using Microsoft.Extensions.Options;

using Notify.Client;
using Notify.Models.Responses;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailService : IEmailService
    {
        private readonly GovNotify _options;
        private readonly string approverEmailTemplateId = "8b70257a-a41c-4260-b9ce-2c6596246fb0";
        private readonly string invoiceRejectedEmailTemplateId = "69bba64c-e5a5-43a4-aab6-179041c21f13";
        private readonly string bulkUploadSuccessEmailTemplateId = "88709ce9-b2b4-4175-a8fa-d0cf354e58b6";
        private readonly string paymentHubErrorTemplateId = "731c5cf4-1cd4-4f07-b0ca-22ea24516bfc";
        private readonly string reportTemplateId = "7b5a3cb8-273d-4da4-a1c4-36e072fb4d1c";

        public EmailService(IOptions<GovNotify> options)
        {
            _options = options.Value;
        }


        public async Task<bool> EmailApprovers(IEnumerable<Approver> approvers, Guid invoiceId, CancellationToken ct)
        {
            var client = new NotificationClient(_options.APIKEY);

            Dictionary<string, dynamic> personalisation = new()
            {
                {"invoiceId", invoiceId.ToString()},
                {"value", "1234.99" },
                { "link", "https://www.bbc.co.uk"}
            };

            foreach (var approver in approvers)
            {
                await client.SendEmailAsync(
                                            emailAddress: approver.Email,
                                            templateId: approverEmailTemplateId,
                                            personalisation: personalisation
                                        );
            }

            return true;
        }

        public async Task<bool> EmailInvoiceRejection(string invoiceCreatorEmail, Guid invoiceId, CancellationToken ct)
        {
            var client = new NotificationClient(_options.APIKEY);

            Dictionary<string, dynamic> personalisation = new()
            {
                {"invoiceId", invoiceId.ToString()},
                {"value", "1234.99" },
                { "link", "https://www.bbc.co.uk"}
            };

            EmailNotificationResponse response = await client.SendEmailAsync(
                                        emailAddress: invoiceCreatorEmail,
                                        templateId: invoiceRejectedEmailTemplateId,
                                        personalisation: personalisation
                                    );

            return response.Equals(true);
        }

        public async Task<bool> EmailBulkUploadSuccess(string invoiceCreatorEmail, string filename, Guid invoiceId, CancellationToken ct)
        {
            var client = new NotificationClient(_options.APIKEY);

            Dictionary<string, dynamic> personalisation = new()
            {
                {"FileName", filename },
                {"confirmationNumber", invoiceId.ToString()},
                { "link", "https://www.bbc.co.uk"}
            };

            EmailNotificationResponse response = await client.SendEmailAsync(
                                        emailAddress: invoiceCreatorEmail,
                                        templateId: bulkUploadSuccessEmailTemplateId,
                                        personalisation: personalisation
                                    );

            return response.Equals(true);
        }

        public async Task<bool> EmailPaymentHubError(string invoiceCreatorEmail, PaymentHubResponseRoot invoiceRequest, CancellationToken ct)
        {
            var client = new NotificationClient(_options.APIKEY);

            Dictionary<string, dynamic> personalisation = new()
            {
                { "invoicerequestid", invoiceRequest!.paymentRequest!.InvoiceRequestId },
                { "error", invoiceRequest.error},
                { "value", invoiceRequest.paymentRequest.invoiceLines!.Sum(x => x.value)}
                //{"invoicedata", JsonSerializer.Serialize(invoiceRequest)}
            };

            EmailNotificationResponse response = await client.SendEmailAsync(
                                        emailAddress: invoiceCreatorEmail,
                                        templateId: paymentHubErrorTemplateId,
                                        personalisation: personalisation
                                    );

            return response.Equals(true);
        }

        public async Task<bool> EmailReport(string recipientEmail, string reportName, byte[] attachment, CancellationToken ct)
        {
            var client = new NotificationClient(_options.APIKEY);

            Dictionary<string, dynamic> personalisation = new()
            {
                { "reportName", reportName },
                { "link_to_file", NotificationClient.PrepareUpload(attachment, reportName, true, "2 weeks")}
            };

            EmailNotificationResponse response = await client.SendEmailAsync(
                                        emailAddress: recipientEmail,
                                        templateId: reportTemplateId,
                                        personalisation: personalisation
                                    );

            return response.Equals(true);
        }
    }
}
