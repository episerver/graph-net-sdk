﻿using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Configuration;
using EPiServer.ContentGraph.Connection;
using EPiServer.Web;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EPiServer.ContentGraph.Extensions
{
    public static class OptiGraphServiceExtension
    {
        public static IServiceCollection AddContentGraphQuery(this IServiceCollection services, Action<OptiGraphOptions>? transformAction = null)
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
            services.AddScoped<GraphQueryBuilder>();

            return services;
        }
    }
}