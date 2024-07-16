using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Invoices.GetAll
{

    internal sealed class GetAllInvoicesResponse
    {
        public IEnumerable<Invoice> Invoices = Enumerable.Empty<Invoice>();
        public string Message { get; set; }
    }
}
