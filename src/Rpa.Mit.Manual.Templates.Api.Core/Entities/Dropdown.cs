using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class Dropdown
    {
        public string Name { get; set; } = string.Empty;
        public string Values { get; set; } = string.Empty;
    }
}
