using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetInvoiceForApproval
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceForApprovalRequest
    {
        public Guid InvoiceId { get; set; }

        internal sealed class Validator : Validator<GetInvoiceForApprovalRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceForApprovalResponse
    {
        public Invoice? Invoice { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
