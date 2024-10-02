using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api
{
    [ExcludeFromCodeCoverage]
    public class PostGres
    {
        public string HOST { get; set; } = string.Empty;
        public string USER { get; set; } = string.Empty;
        public string DATABASE { get; set; } = string.Empty;
        public string TOKEN_SCOPES { get; set; } = string.Empty;
        public string MANAGEDIDENTITYCLIENTID { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class PaymentHub
    {
        public string CONNECTION { get; set; } = string.Empty;
        public string TOPIC { get; set; } = string.Empty;
        public string RESPONSE_TOPIC { get; set; } = string.Empty;
        public string RESPONSE_SUBSCRIPTION { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class GovNotify
    {
        public string APIKEY { get; set; } = string.Empty;
    }
}
