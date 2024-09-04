
using Rpa.Mit.Manual.Templates.Api.Api.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    /// <summary>
    /// scheduled service to return payment hub responses to the business.
    /// </summary>
    public class FeedbackToBusinessBackgroundService : BackgroundService, IAsyncDisposable
    {
        private readonly ILogger<FeedbackToBusinessBackgroundService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(15);
        private readonly IServiceScopeFactory _factory;

        public FeedbackToBusinessBackgroundService(
            ILogger<FeedbackToBusinessBackgroundService> logger, 
            IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    // We cannot use the default dependency injection behavior, because ExecuteAsync is
                    // a long-running method while the background service is running.
                    // To prevent open resources and instances, only create the services and other references on a run

                    // Create scope, so we get request services
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

                    // Get service from scope
                    var businessFeedbackService = asyncScope.ServiceProvider.GetRequiredService<BusinessFeedbackService>();

                    await businessFeedbackService.SendPaymentHubResponses();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Message}", ex.Message);
                }
            }
        }



        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
