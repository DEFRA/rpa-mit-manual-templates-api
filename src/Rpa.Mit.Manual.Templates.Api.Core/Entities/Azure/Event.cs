namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    public class Event
    {
        public string Name { get; init; } = default!;

        public EventProperties Properties { get; init; } = default!;
    }
}
