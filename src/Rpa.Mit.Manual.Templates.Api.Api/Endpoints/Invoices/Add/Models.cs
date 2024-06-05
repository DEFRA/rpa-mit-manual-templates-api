using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Invoices.Add
{
    internal sealed class Request
    {
        public Invoice Invoice { get; set; }
    }

    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;

        public Invoice? Invoice { get; set; }
    }
}
