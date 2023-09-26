namespace EPiServer.ContentGraph.Connection
{
    public class CacheAccessor
    {
        private readonly ICache _cache;

        public CacheAccessor(ICache cache)
        {
            _cache = cache;
        }

        public static ICache Cache { get; set; }
    }
}
