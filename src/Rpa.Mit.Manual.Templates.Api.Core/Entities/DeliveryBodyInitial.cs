using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// used in the initial invoice header screen
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class DeliveryBodyInitial
    {
        public string Code { get; set; } = string.Empty;
        public string DeliveryBodyDescription { get; set; } = string.Empty;
        public string AccountCode { get; set; } = string.Empty;
        public string Org { get; set; } = string.Empty;
    }
}
