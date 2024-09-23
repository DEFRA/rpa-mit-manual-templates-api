using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace ApproveInvoiceAr
{
    /// <summary>
    /// approve an AR invoice
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceAr : EndpointWithMapping<ApproveInvoiceArRequest, ApproveInvoiceArResponse, InvoiceApproval>
    {
        public override void Configure()
        {
            Post("/approvals/approvear");
        }

        public override async Task HandleAsync(ApproveInvoiceArRequest r, CancellationToken c)
        {
            await SendAsync(new ApproveInvoiceArResponse());
        }

        public sealed override async Task<InvoiceApproval> MapToEntityAsync(ApproveInvoiceArRequest r, CancellationToken ct = default)
        {
            var invoiceApproval = await Task.FromResult(new InvoiceApproval());

            invoiceApproval.ApproverEmail = User.Identity?.Name!;
            invoiceApproval.DateApproved = DateTime.UtcNow;
            invoiceApproval.Id = r.Id;

            return invoiceApproval;
        }
    }
}