using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public class EventAction
    {
        public string Type { get; init; } = default!;

        public string Message { get; init; } = default!;

        public DateTime Timestamp { get; init; } = default!;

        public string Data { get; init; } = default!;
    }
}
