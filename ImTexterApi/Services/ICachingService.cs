namespace ImTexterApi.Services
{
    public interface ICachingService
    {
        T? GetCacheData<T>(string key);

        void SetCacheData<T>(string key, T data, TimeSpan timeSpan);
        object ClearApiCacheData(string key);
        void ClearCacheData(string key);

    }
}
