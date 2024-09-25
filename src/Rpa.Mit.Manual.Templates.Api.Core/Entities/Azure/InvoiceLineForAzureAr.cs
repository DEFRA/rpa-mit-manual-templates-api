using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public sealed record InvoiceLineForAzureAr : InvoiceLineForAzure
    {
        public string debtType { get; set; } = string.Empty;
    }
}
