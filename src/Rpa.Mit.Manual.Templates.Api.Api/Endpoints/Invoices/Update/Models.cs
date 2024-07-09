using System.Diagnostics.CodeAnalysis;

namespace Invoices.Update
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public string Message { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
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
    }
}
