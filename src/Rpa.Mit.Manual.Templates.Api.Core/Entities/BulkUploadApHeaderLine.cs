using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadApHeaderLine : BulkUploadBaseClass
    {
        public string CustomerId { get; set; } = string.Empty;

        /// <summary>
        /// Calulated field
        /// </summary>
        public decimal TotalAmount { get; set; } = 0.0M;

        public string Description { get; set; } = string.Empty;
    }
}
