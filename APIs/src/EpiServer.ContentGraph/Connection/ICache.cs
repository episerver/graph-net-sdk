namespace EPiServer.ContentGraph.Connection
{
    public interface ICache
    {
        TCachedObject Get<TCachedObject>(string key);

        void Add(string key, StaticCachePolicy cachePolicy, object value);

        void AddOrUpdate(string key, StaticCachePolicy cachePolicy, object value);
    }
}