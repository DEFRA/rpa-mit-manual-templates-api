using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public string GovNotifyApiKey { get; set; } = string.Empty;
        public string EventQueueName { get; set; } = string.Empty;
        public string ImporterQueueName { get; set; } = string.Empty;
        public string BlobContainerName { get; set; } = string.Empty;
        public string BlobConnectionString { get; set; } = string.Empty;
        public string NotificationQueueName { get; set; } = string.Empty;
        public string ServiceBusConnectionString { get; set; } = string.Empty;
        public string InvoiceAPIBaseURI { get; set; } = string.Empty;
        public string ApprovalAPIBAseURI { get; set; } = string.Empty;
        public string ReferenceDataAPIBaseURI { get; set; } = string.Empty;
        public string InvoiceImporterAPIBaseURI { get; set; } = string.Empty;
        public string ApplicationBaseUri { get; set; } = string.Empty;
        public string PaymentGeneratorQueueName { get; set; } = string.Empty;
        public string PaymentHubTopicConnectionString { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class PostGres
    {
        public string Host { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string TokenScopes { get; set; } = string.Empty;
        public string ManagedIdentityClientId { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class ConnectionStrings
    {
        public string PrimaryConnection { get; set; } = string.Empty;
        public string PostGres { get; set; } = string.Empty;
        public string PostGresLocal { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class PaymentHub
    {
        public string TopicConnectionString { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class GovNotify
    {
        public string ApiKey { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class AzureAd
    {
        public string Instance { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string CallbackPath { get; set; } = string.Empty;
    }
}
