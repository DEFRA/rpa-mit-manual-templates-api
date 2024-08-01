using System.Diagnostics.CodeAnalysis;

using Azure.Core;
using Azure.Identity;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints
{
    [ExcludeFromCodeCoverage]
    public class BaseData
    {
        private static readonly string Host = "sndadpdbsps4401.postgres.database.azure.com";
        private static readonly string User = "AAG-Azure-ADP-lap-mit-snd4-PostgresDB_Writer";
        private static readonly string Database = "lap-mit-invoices";
        private static readonly string ManagedIdentityClientId = "cdc04155-34c7-44f9-b657-b6e4f084be15";


        protected BaseData() 
        {}

        public async Task<string> DbConn()
        {
            var tokenProvider = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = ManagedIdentityClientId
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
