using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public class Event
    {
        public string Name { get; init; } = default!;

        public EventProperties Properties { get; init; } = default!;
    }
}
