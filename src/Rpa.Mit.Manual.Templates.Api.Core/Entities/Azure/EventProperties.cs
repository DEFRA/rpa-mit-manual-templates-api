namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    public class EventProperties
    {
        public string Id { get; init; } = default!;

        public string Checkpoint { get; init; } = default!;

        public string Status { get; init; } = default!;

        public EventAction Action { get; init; } = default!;
    }
}
