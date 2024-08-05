using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace RejectInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : EndpointWithMapping<RejectInvoiceRequest, RejectInvoiceResponse, InvoiceRejection>
    {
        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/approvals/reject");
        }

        public override async Task HandleAsync(RejectInvoiceRequest r, CancellationToken ct)
        {
            await SendAsync(new RejectInvoiceResponse(), 200, CancellationToken.None);
        }

        public sealed override async Task<InvoiceRejection> MapToEntityAsync(RejectInvoiceRequest r, CancellationToken ct = default)
        {
            var invoiceRejection = await Task.FromResult(new InvoiceRejection());

            invoiceRejection.RejectedBy = "aylmer.carson";
            invoiceRejection.DateRejected = DateTime.UtcNow;
            invoiceRejection.Reason = r.Reason;
            invoiceRejection.Id = r.Id;

            return invoiceRejection;
        }
    }
}