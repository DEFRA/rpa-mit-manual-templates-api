using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Dropdowns.GetAll
{
    internal sealed class Response
    {
        public IEnumerable<Dropdown> Dropdowns { get; set; } = Enumerable.Empty<Dropdown>();

        public string Message { get; set; } = string.Empty;
    }
}
