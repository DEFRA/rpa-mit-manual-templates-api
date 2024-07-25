using System.Diagnostics.CodeAnalysis;

namespace ApproveInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {


        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;
    }
}
