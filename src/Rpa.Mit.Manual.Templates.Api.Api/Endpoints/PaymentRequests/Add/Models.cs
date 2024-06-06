using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace PaymentsRequests.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public PaymentRequest PaymentRequest { get; set; }
    }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; } = false;
    }
}
