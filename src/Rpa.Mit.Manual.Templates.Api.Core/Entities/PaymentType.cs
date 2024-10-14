using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// This is atually currency. Current naming is confusing, copied from legacy code. 
    /// Should be renamed to Currency.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class PaymentType
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
