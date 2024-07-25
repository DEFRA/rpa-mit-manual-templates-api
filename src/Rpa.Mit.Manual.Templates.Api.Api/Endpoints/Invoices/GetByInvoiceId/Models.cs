using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetByInvoiceIdRequest
    {
        public Guid InvoiceId { get; set; }

        internal sealed class Validator : Validator<GetByInvoiceIdRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetByInvoiceIdResponse
    {
        public Invoice? Invoice { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
