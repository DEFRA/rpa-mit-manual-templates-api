namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure
{
    public interface IServiceBusProvider
    {
        Task SendMessageAsync(string queue, string msg);
    }
}
