using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class SchemeType
    {
        /// <summary>
        /// this is the value in the click handler in the VBA code. included here to help development.
        /// </summary>
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string DeliveryBodyCode { get; set; } = string.Empty;    
    }
}
