using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetPaymentTypes
{
    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;
        public IEnumerable<PaymentType> PaymentTypes { get; set; } = Enumerable.Empty<PaymentType>();
    }
}
