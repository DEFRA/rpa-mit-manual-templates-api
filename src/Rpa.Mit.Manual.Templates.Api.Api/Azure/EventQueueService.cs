using System.Text.Json;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    public class EventQueueService : IEventQueueService
    {
        private readonly IOptions<AppSettings> _options;
        private readonly IServiceBusProvider _iServiceBusProvider;

        public EventQueueService(
            IOptions<AppSettings> options, 
            IServiceBusProvider iServiceBusProvider)
        {
            _options = options;
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

            await _iServiceBusProvider.SendMessageAsync(_options.Value.EventQueueName, JsonSerializer.Serialize(eventRequest));

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

            try
            {
                await _iServiceBusProvider.SendMessageAsync(_options.Value.EventQueueName, JsonSerializer.Serialize(eventRequest));
                return true;

            }
            catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
            {
                // Ignore any errors if the queue already exists
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error {ex.Message} sending \"{message}\" message to Event Queue.");
                return false;
            }
        }
    }
}
