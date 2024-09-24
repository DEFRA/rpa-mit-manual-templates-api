using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetInvoiceForApprovalAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceForApprovalArRequest
    {
        public Guid InvoiceId { get; set; }

        internal sealed class Validator : Validator<GetInvoiceForApprovalArRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceForApprovalArResponse
    {
        public InvoiceAr? InvoiceAr { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
