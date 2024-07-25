using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
!
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    [ExcludeFromCodeCoverage]
    public class EventQueueService : IEventQueueService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceBusProvider _iServiceBusProvider;

        public EventQueueService(
            IServiceBusProvider iServiceBusProvider,
            IConfiguration configuration )
        {
            _configuration = configuration;
            _iServiceBusProvider = iServiceBusProvider;
        }

        public async Task<bool> CreateMessage(Invoice invoice, string message)
        {
            var eventRequest = new Event()
            {
                Name = "Invoice",
                Properties = new EventProperties()
                {
                    Id = invoice.Id.ToString(),
                    Status = invoice.Status,
                    Checkpoint = "Invoice Api",
                    Action = new EventAction()
                    {
                        Type = "invoice-created",
                        Message = message,
                        Timestamp = DateTime.UtcNow,
                        Data = JsonSerializer.Serialize(invoice)
                    }
                }
            };

            await _iServiceBusProvider.SendMessageAsync(_configuration.GetSection("EventQueueName").Value!, JsonSerializer.Serialize(eventRequest));

            return true;
        }


        public async Task<bool> AddMessageToQueueAsync(string message, string data)
        {
            var eventRequest = new Event()
            {
                Name = "Payments",
                Properties = new EventProperties()
                {
                    Status = "",
                    Checkpoint = "",
                    Action = new EventAction()
                    {
                        Type = "",
                        Message = message,
                        Timestamp = DateTime.UtcNow,
                        Data = data
                    }
                }
            };


            await _iServiceBusProvider.SendMessageAsync(_configuration.GetSection("EventQueueName").Value!, JsonSerializer.Serialize(eventRequest));
                
            return true;
        }
    }
}
