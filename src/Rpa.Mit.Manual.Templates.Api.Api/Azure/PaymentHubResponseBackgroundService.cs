using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

using static System.Threading.CancellationTokenSource;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    /// <summary>
    /// monitors the payment hub servicebus for responses to uploads
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public class PaymentHubBackgroundService<T> : BackgroundService, IAsyncDisposable
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IMessageHandler<T> _handler;
        private readonly ILogger<PaymentHubBackgroundService<T>> _logger;
        private CancellationTokenSource? stoppingCts;

        public PaymentHubBackgroundService(
                ILogger<PaymentHubBackgroundService<T>> logger,
                ServiceBusProcessor processor,
                IMessageHandler<T> handler
            )
        {
            _logger = logger;
            _processor = processor;
            _handler = handler;

            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            var obj = args.Message.Body.ToObjectFromJson<T>();

            var cts = CreateLinkedTokenSource(stoppingCts!.Token, args.CancellationToken);

            await _handler.HandleAsync(obj, cts.Token);

            cts.Dispose();

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError("Error Processing {@Error}",
                new
                {
                    args.Identifier,
                    ErrorSource = $"{args.ErrorSource}",
                    Exception = $"{args.Exception}"
                });

            return Task.CompletedTask;
        }

        protected static Task CompleteOnCancelAsync(CancellationToken token)
        {
            var tcs = new TaskCompletionSource();

            token.Register(t =>
            {
                if (t is TaskCompletionSource tcs)
                    tcs.TrySetResult();
            }, tcs);

            return tcs.Task;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingCts = CreateLinkedTokenSource(stoppingToken);
            await _processor.StartProcessingAsync(CancellationToken.None);

            await CompleteOnCancelAsync(stoppingToken);

            await stoppingCts.CancelAsync();

            await _processor.StopProcessingAsync(CancellationToken.None);
        }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await _processor.DisposeAsync();
            stoppingCts?.Dispose();
            base.Dispose();
        }
    }
}
