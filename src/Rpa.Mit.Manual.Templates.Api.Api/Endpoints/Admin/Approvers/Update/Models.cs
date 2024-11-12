using System.Diagnostics.CodeAnalysis;

namespace UpdateApprover
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public required string Email { get; set; }
        public required string DeliveryBody { get; set; }
        public required string SchemeCode { get; set; }
        public required int Threshold { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
