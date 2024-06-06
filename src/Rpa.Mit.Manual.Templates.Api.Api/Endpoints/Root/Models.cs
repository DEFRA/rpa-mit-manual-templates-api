using System.Diagnostics.CodeAnalysis;

namespace Root
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {

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
        public string Message => "This endpoint hasn't been implemented yet!";
    }
}
