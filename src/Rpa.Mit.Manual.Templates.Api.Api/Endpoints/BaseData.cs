using System.Diagnostics.CodeAnalysis;

using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Options;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseData
    {
        // Obtain connection string information from the portal for use in the following variables
        private static readonly string Host = "sndadpdbsps4401.postgres.database.azure.com";
        private static readonly string User = "AAG-Azure-ADP-lap-mit-snd4-PostgresDB_Writer";
        private static readonly string Database = "lap-mit-invoices";

        private readonly IOptions<ConnectionStrings> _options;

        protected BaseData(IOptions<ConnectionStrings> options) 
        {
            _options = options;
        }

        public virtual string DbConn => _options.Value.PostGres;

        public virtual async Task<string> GetConnectionStringUsingManagedIdentity()
        {
            // For user-assigned identity.
            var tokenProvider = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = "cdc04155-34c7-44f9-b657-b6e4f084be15"
                });

            AccessToken accessToken = await tokenProvider.GetTokenAsync(
                new TokenRequestContext(scopes: new string[]
                {
                    "https://ossrdbms-aad.database.windows.net/.default"
                }));

            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4}; SSLMode=Require",
                    Host,
                    User,
                    Database,
                    5432,
                    accessToken.Token);

            return connString;
        }
    }
}
