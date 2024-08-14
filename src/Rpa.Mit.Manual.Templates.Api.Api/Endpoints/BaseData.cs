using System.Diagnostics.CodeAnalysis;

using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Options;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints
{
    [ExcludeFromCodeCoverage]
    public class BaseData
    {
        private readonly PostGres _options;
        private static readonly string _tokenScopes = "https://ossrdbms-aad.database.windows.net/.default";

        public BaseData(IOptions<PostGres> options)
        {
            _options = options.Value;
        }

        public async Task<string> DbConn()
        {
            var tokenProvider = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = _options.MANAGEDIDENTITYCLIENTID,
                });

            AccessToken accessToken = await tokenProvider.GetTokenAsync(
                new TokenRequestContext(scopes: new string[]
                {
                     _tokenScopes
                }));

            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4}; SSLMode=Require",
                    _options.HOST,
                    _options.USER,
                    _options.DATABASE,
                    5432,
                    accessToken.Token);

            return connString;
        }
    }
}
