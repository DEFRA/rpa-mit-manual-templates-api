using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities;

[ExcludeFromCodeCoverage]
public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

