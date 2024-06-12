using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure
{
    public interface IEventQueueService
    {
        Task<bool> CreateMessage(Invoice invoice, string message);

        Task<bool> AddMessageToQueueAsync(string message, string data);
    }
}
