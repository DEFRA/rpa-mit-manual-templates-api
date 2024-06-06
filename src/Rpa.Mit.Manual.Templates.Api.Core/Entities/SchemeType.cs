using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class SchemeType
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
