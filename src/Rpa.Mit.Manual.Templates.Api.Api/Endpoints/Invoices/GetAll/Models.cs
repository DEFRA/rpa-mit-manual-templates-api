using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Invoices.GetAll
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetAllInvoicesResponse
    {
        public IEnumerable<Invoice> Invoices { get; set; } = Enumerable.Empty<Invoice>();
        public string Message { get; set; } = string.Empty;
    }
}
