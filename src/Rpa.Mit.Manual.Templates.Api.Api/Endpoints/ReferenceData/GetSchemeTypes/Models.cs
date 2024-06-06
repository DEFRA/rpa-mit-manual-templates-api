using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetSchemeTypes;

[ExcludeFromCodeCoverage]
internal sealed class Response
{
    public string Message { get; set; } = string.Empty;
    public IEnumerable<SchemeType> SchemeTypes { get; set; } = Enumerable.Empty<SchemeType>();
}

