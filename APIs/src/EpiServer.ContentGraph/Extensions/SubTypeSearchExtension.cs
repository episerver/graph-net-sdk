using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.ContentGraph.Extensions
{
    public static class SubTypeSearchExtension
    {
        #region SubTypeQueryBuilder
        public static SubTypeQueryBuilder<T> GetDeleted<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.DELETED);
        }
        public static SubTypeQueryBuilder<T> GetId<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.ID);
        }
        public static SubTypeQueryBuilder<T> GetModified<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.MODIFIED);
        }
        public static SubTypeQueryBuilder<T> GetScore<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.SCORE);
        }
        public static SubTypeQueryBuilder<T> GetTypeName<T>(this SubTypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.TYPE_NAME);
        }
        #endregion
    }
}
