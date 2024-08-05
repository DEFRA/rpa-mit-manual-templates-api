using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals
{
    public class EmailService : IEmailService
    {
        private readonly GovNotify _options;

        public EmailService(IOptions<GovNotify> options)
        {
            _options = options.Value;
        }

        
        public async Task<bool> EmailApprovers(IEnumerable<string> approvers, CancellationToken ct)
        {
            return true;
        }
    }
}
