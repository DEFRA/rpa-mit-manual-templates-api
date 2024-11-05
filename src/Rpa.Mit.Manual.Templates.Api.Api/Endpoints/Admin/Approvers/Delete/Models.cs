using System.Diagnostics.CodeAnalysis;

namespace DeleteApprover
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public required string Email { get; set; }

        public required string DeliveryBody { get; set; }

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
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
