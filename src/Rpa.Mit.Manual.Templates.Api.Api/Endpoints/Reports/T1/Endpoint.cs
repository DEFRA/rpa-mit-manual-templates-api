using System.Diagnostics.CodeAnalysis;

using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Reports;

namespace TiReport
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : EndpointWithoutRequest
    {
        private readonly ILogger<Endpoint> _logger;
        private readonly PostGres _options;
        private static readonly string _tokenScopes = "https://ossrdbms-aad.database.windows.net/.default";
        private static readonly string _filePath = @"C:\\Reports\\GeneratedT1.csv";

        public Endpoint(
            ILogger<Endpoint> logger,
            IOptions<PostGres> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("reports/t1");
        }

        public override async Task HandleAsync(CancellationToken ct)
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

            var response = new Response();

            try
            {
                response.Report = Data.GetT1ReportData(connString, ct);

                var reportAsCsv = response.Report.AsCsv<T1Report>();

                await File.WriteAllTextAsync(_filePath, reportAsCsv);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
        }
    }
}