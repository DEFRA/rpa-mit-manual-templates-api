namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure
{
    public interface IMessageHandler<T>
    {
        public Task<Task> HandleAsync(T message, CancellationToken cancelToken);
    }
}
