using EPiServer.ContentGraph.Connection;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

namespace EpiServer.ContentGraph.UnitTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICache, RuntimeCacheAdapter>();
            var provider = services.BuildServiceProvider();
            CacheAccessor.Cache = provider.GetService<ICache>();
        }
    }
}
