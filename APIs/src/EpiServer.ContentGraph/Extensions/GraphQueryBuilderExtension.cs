using Azure.Core;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Tracing;

namespace EPiServer.ContentGraph.Extensions
{
    public static class GraphQueryBuilderExtension
    {
        #region Find adaptation
        public static TypeQueryBuilder<T> Search<T>(this GraphQueryBuilder queryBuilder)
        {
            return queryBuilder.ForType<T>();
        }
        public static TypeQueryBuilder<T> Search<T>(this GraphQueryBuilder queryBuilder, TypeQueryBuilder<T> typeQueryBuilder)
        {
            return queryBuilder.ForType(typeQueryBuilder);
        }
        #endregion
        public static TypeQueryBuilder<T> BeginType<T>(this GraphQueryBuilder queryBuilder)
        {
            return queryBuilder.ForType<T>();
        }
        public static TypeQueryBuilder<T> BeginType<T>(this GraphQueryBuilder queryBuilder, TypeQueryBuilder<T> typeQueryBuilder)
        {
            return queryBuilder.ForType(typeQueryBuilder);
        }
        /// <summary>
        /// Search result has single item will be cached. This will set cache_unique to true and require cache=true in graph client options.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public static GraphQueryBuilder SingleResultCache(this GraphQueryBuilder queryBuilder)
        {
            queryBuilder.GraphOptionsAction = options =>
            {
                if (options.QueryPath.Contains("cache=true"))
                {
                    options.QueryPath += "&cache_uniq=true";
                }
                else
                {
                    //log warning messge
                    Trace.Instance.Add(new TraceEvent(queryBuilder, "cache_uniq is set to true but cache is not enable. Please enable it in the config before use."));
                }
            };
            return queryBuilder;
        }
    }
}
