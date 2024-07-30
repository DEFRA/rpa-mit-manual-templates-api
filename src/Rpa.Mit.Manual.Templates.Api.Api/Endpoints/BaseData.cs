using System.Diagnostics.CodeAnalysis;

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
            string accessToken = string.Empty;

            try
            {
                // Call managed identities for Azure resources endpoint.
                var tokenProvider = new DefaultAzureCredential();

                accessToken = (await tokenProvider.GetTokenAsync(
                    new Azure.Core.TokenRequestContext(
                        scopes: new string[] { "https://ossrdbms-aad.database.windows.net/.default" }) { }))
                        .Token;

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : "Acquire token failed");
                System.Environment.Exit(1);
            }

            //
            // Open a connection to the PostgreSQL server using the access token.
            //
            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4}; SSLMode=Prefer",
                    Host,
                    User,
                    Database,
                    5432,
                    accessToken);

            return connString;
        }
    }
}
