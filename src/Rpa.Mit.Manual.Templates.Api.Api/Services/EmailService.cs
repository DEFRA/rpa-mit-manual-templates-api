using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Notify.Client;
using Notify.Models.Responses;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailService : IEmailService
    {
        private readonly GovNotify _options;
        private readonly string approverEmailTemplateId = "8b70257a-a41c-4260-b9ce-2c6596246fb0";
        private readonly string invoiceRejectedEmailTemplateId = "69bba64c-e5a5-43a4-aab6-179041c21f13";

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
    }
}
