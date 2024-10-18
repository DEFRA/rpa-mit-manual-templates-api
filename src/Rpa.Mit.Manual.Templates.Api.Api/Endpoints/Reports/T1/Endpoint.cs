﻿using System.Diagnostics.CodeAnalysis;
using System.Text;

using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Reports;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace T1Report
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : EndpointWithoutRequest
    {
        private readonly IEmailService _iEmailService;
        private readonly ILogger<Endpoint> _logger;
        private readonly PostGres _options;
        private static readonly string _tokenScopes = "https://ossrdbms-aad.database.windows.net/.default";

        public Endpoint(
            IEmailService iEmailService,
            ILogger<Endpoint> logger,
            IOptions<PostGres> options)
        {
            _iEmailService = iEmailService;
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
                }), CancellationToken.None);

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
                response.Report = Data.GetT1ReportData(connString);

                var reportAsCsv = response.Report.AsCsv<T1Report>();

                await _iEmailService.EmailReport("aylmer.carson.external@eviden.com", "MIT_Report_T1.csv", Encoding.ASCII.GetBytes(reportAsCsv), ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
        }
    }
}