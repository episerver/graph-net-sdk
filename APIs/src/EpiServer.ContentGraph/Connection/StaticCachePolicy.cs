using Microsoft.Extensions.Primitives;
using System;

namespace EPiServer.ContentGraph.Connection
{
    public class StaticCachePolicy
    {
        public StaticCachePolicy()
        {
            Duration = TimeSpan.Zero;
            ExpirationDate = DateTime.MaxValue;
        }

        public StaticCachePolicy(TimeSpan slidingExpiration)
        {
            Duration = slidingExpiration;
            ExpirationDate = DateTime.MaxValue;
        }

        public StaticCachePolicy(DateTime expirationDate)
        {
            Duration = TimeSpan.MaxValue;
            ExpirationDate = expirationDate;
        }

        public TimeSpan Duration { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public IChangeToken ChangeToken { get; set; }

        public string CacheDependencyKey { get; set; }
    }
}
