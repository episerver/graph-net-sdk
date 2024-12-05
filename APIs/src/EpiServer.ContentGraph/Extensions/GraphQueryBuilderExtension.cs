using EPiServer.ContentGraph.Api.Querying;

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
            queryBuilder.RequestActions = request => request.AddRequestHeader("cache_uniq", "true");
            return queryBuilder;
        }
    }
}
