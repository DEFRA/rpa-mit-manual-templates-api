using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetOrganisations
{
    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;
        public IEnumerable<Organisation> Organisations { get; set; } = Enumerable.Empty<Organisation>();
    }
}
