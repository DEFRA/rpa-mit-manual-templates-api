using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Integration.Tests
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
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
    }

    [ExcludeFromCodeCoverage]
    public class ConnectionStrings
    {
        public string PrimaryConnection { get; set; } = string.Empty;
        public string PostGres { get; set; } = "Server=127.0.0.1;Database=rpa_mit_invoices;Username=postgres;Password=ghjiUIbfds345;";
        public string PostGresLocal { get; set; } = string.Empty;
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
