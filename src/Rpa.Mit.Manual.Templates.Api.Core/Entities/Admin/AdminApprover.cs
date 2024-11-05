using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin
{
    [ExcludeFromCodeCoverage]
    public class AdminApprover
    {
        public string Email { get; set; } = string.Empty;
        public string DeliveryBody { get; set; } = string.Empty;
        public string SchemeCode { get; set; } = string.Empty;
        public int? Threshold { get; set; } 
    }
}
