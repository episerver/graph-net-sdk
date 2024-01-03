using EPiServer.ContentGraph.Api.Querying;

namespace EPiServer.ContentGraph.Extensions
{
    public static class SearchExtension
    {
        #region TypeQueryBuilder
        public static TypeQueryBuilder<T> GetDeleted<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_deleted");
        }
        public static TypeQueryBuilder<T> GetId<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_id");
        }
        public static TypeQueryBuilder<T> GetModified<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_modified");
        }
        public static TypeQueryBuilder<T> GetScore<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_score");
        }
        public static TypeQueryBuilder<T> GetTypeName<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("__typename");
        }
        #endregion

        #region SubTypeQueryBuilder
        public static SubTypeQueryBuilder<T> GetDeleted<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_deleted");
        }
        public static SubTypeQueryBuilder<T> GetId<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_id");
        }
        public static SubTypeQueryBuilder<T> GetModified<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_modified");
        }
        public static SubTypeQueryBuilder<T> GetScore<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("_score");
        }
        public static SubTypeQueryBuilder<T> GetTypeName<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field("__typename");
        }
        #endregion
    }
}
