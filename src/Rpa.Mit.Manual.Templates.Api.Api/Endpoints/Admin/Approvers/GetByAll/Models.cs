using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;

namespace ApproversGetByAll
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public string? Email { get; set; }
        public string? DeliveryBody { get; set; }
        public string? SchemeCode { get; set; }
        public int? Threshold { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public IEnumerable<AdminApprover> AdminApprovers { get; set; } = Enumerable.Empty<AdminApprover>();

        public string Message { get; set; } = string.Empty;
    }
}
