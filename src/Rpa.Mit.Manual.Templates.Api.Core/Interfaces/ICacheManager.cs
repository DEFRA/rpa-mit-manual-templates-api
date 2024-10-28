namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface ICacheManager
    {
        T Get<T>(string key);

        void Set(string key, object data, int cacheTime);

        bool IsSet(string key);

        void Remove(string key);

        void Clear();
    }
}
