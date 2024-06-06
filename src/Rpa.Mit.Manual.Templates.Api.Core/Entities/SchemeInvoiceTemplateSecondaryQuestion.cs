using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class SchemeInvoiceTemplateSecondaryQuestion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
