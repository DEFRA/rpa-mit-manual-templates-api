﻿using GetPaymentTypes;
using Microsoft.Extensions.Caching.Memory;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetSchemeTypes
{
    internal sealed class GetSchemeTypesEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<GetSchemeTypesEndpoint> _logger;

        public GetSchemeTypesEndpoint(
                       ILogger<GetSchemeTypesEndpoint> logger,
                       IMemoryCache memoryCache,
                       IReferenceDataRepo iReferenceDataRepo)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Get("/schemetypes/get");
            ResponseCache(600); //cache seconds
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                response.SchemeTypes = await _iReferenceDataRepo.GetSchemeTypeReferenceData(ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}