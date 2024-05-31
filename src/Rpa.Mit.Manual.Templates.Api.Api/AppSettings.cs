﻿namespace Rpa.Mit.Manual.Templates.Api
{
    public class AppSettings
    {
        public string EventQueueName { get; set; } = string.Empty;
        public string ImporterQueueName { get; set; } = string.Empty;
        public string BlobContainerName { get; set; } = string.Empty;
        public string BlobConnectionString { get; set; } = string.Empty;
        public string NotificationQueueName { get; set; } = string.Empty;
        public string QueueConnectionString { get; set; } = string.Empty;
        public string InvoiceAPIBaseURI { get; set; } = string.Empty;
        public string ApprovalAPIBAseURI { get; set; } = string.Empty;
        public string ReferenceDataAPIBaseURI { get; set; } = string.Empty;
        public string InvoiceImporterAPIBaseURI { get; set; } = string.Empty;
        public string ApplicationBaseUri { get; set; } = string.Empty;
        public string PaymentGeneratorQueueName { get; set; } = string.Empty;

        //public ConnectionStrings? ConnectionStrings { get; set; }

        //public AzureAd? AzureAd { get; set; }
    }

    public class ConnectionStrings
    {
        public string PrimaryConnection { get; set; } = string.Empty;
        public string PostGres { get; set; } = string.Empty;
        public string PostGresLocal { get; set; } = string.Empty;
    }


    public class AzureAd
    {
        public string Instance { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string CallbackPath { get; set; } = string.Empty;
    }
}
