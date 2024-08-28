namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure
{
    public interface IServiceBusTopicSubscription
    {
        Task PrepareFiltersAndHandleMessages();
        Task CloseSubscriptionAsync();
        ValueTask DisposeAsync();
    }
}
