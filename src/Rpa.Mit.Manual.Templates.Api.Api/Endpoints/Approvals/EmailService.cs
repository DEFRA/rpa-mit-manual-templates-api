using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Notify.Client;
using Notify.Models.Responses;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals
{
    [ExcludeFromCodeCoverage]
    public class EmailService : IEmailService
    {
        private readonly GovNotify _options;
        private readonly string approverEmailTemplateId = "f33517ff-2a88-4f6e-b855-c550268ce08a";

        public EmailService(IOptions<GovNotify> options)
        {
            _options = options.Value;
        }

        
        public async Task<bool> EmailApprovers(IEnumerable<string> approvers, CancellationToken ct)
        {
            var client = new NotificationClient(_options.API_KEY);

            EmailNotificationResponse response = await client.SendEmailAsync(
                                                        emailAddress: "sender@something.com",
                                                        templateId: approverEmailTemplateId
                                                    );
            return response.content != null;
        }
    }
}
