using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    ///  from the lookup_accounts_ar table
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record AccountAr
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Org { get; set; } = string.Empty;
    }
}
