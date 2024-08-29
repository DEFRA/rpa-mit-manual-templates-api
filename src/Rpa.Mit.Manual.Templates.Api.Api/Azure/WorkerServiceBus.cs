﻿using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    [ExcludeFromCodeCoverage]
    public class WorkerServiceBus : IHostedService, IDisposable
    {
        private readonly ILogger<WorkerServiceBus> _logger;
        private readonly IServiceBusTopicSubscription _serviceBusTopicSubscription;

        public WorkerServiceBus(
            IServiceBusTopicSubscription serviceBusTopicSubscription,
            ILogger<WorkerServiceBus> logger)
        {
            _serviceBusTopicSubscription = serviceBusTopicSubscription;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _serviceBusTopicSubscription.PrepareFiltersAndHandleMessages();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await _serviceBusTopicSubscription.CloseSubscriptionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await _serviceBusTopicSubscription.DisposeAsync();
            }
        }
    }
}
