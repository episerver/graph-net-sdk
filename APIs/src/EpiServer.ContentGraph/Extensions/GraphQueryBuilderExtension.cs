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
    }
}
