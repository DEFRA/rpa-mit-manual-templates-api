using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace PaymentsRequests.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public required PaymentRequest PaymentRequest { get; set; }
    }


    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; } = false;
    }
}
