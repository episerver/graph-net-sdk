using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Configuration;
using EPiServer.ContentGraph.Connection;
using EPiServer.Web;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;

namespace EPiServer.ContentGraph.Extensions
{
    public static class OptiGraphServiceExtension
    {
        public static IServiceCollection AddContentGraphCore(this IServiceCollection services, Action<OptiGraphOptions>? transformAction = null)
        {
            var optionsBuilder = services
                .AddOptions<OptiGraphOptions>()
                .BindConfiguration(OptiGraphOptions.ConfigSection);

            if (transformAction != null)
            {
                optionsBuilder.Configure(transformAction);
            }

            optionsBuilder.Configure(options =>
            {
                options.GatewayAddress = VirtualPathUtilityEx.AppendTrailingSlash(options.GatewayAddress);
                options.QueryPath = options.QueryPath.TrimStart('/');
            });

            CacheAccessor.Cache = services.BuildServiceProvider()?.GetService<ICache>();
            if (CacheAccessor.Cache == null)
            {
                var cache = new RuntimeCacheAdapter(services.BuildServiceProvider().GetService<IMemoryCache>());
                services.TryAddSingleton<ICache>(cache);
                CacheAccessor.Cache = cache;
            }
            services.AddScoped<IFilterForVisitor,FilterDeletedForVisitor>();
            services.AddScoped<GraphQueryBuilder>();
            services.AddHttpClient("HttpClientWithAutoDecompression", c => { })
                .ConfigurePrimaryHttpMessageHandler(() => {
                    var httpClientHandler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                    };
                    return httpClientHandler;
                });

            return services;
        }
    }
}
