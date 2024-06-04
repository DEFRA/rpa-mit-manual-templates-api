using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace PaymentsRequests.Add
{
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

    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; } = false;
    }
}
