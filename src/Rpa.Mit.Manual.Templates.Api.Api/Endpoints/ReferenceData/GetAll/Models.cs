using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;

[ExcludeFromCodeCoverage]
internal sealed class Response
{
    public string Message { get; set; } = string.Empty;

    public ReferenceData? ReferenceData { get; set; }
}

