using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// an invoice detail line for AR invoices
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceLineAr : InvoiceLineBase
    {
        /// <summary>
        /// calculated. either ‘ADMIN ERROR’ or ‘IRREGULARITY’
        /// </summary>
        public string DebtType { get; set; } = string.Empty;
    }
}
