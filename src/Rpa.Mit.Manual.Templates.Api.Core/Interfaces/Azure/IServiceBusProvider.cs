namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure
{
    public interface IServiceBusProvider
    {
        Task SendInvoiceRequestJson(string msg);
    }
}
