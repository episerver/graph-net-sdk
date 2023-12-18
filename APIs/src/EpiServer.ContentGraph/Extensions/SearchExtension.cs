using EPiServer.ContentGraph.Api.Querying;

namespace EPiServer.ContentGraph.Extensions
{
    public static class SearchExtension
    {
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
    }
}
