using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Helpers;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Extensions
{
    public static class TypeSearchExtension
    {
        #region TypeQueryBuilder
        public static TypeQueryBuilder<T> GetDeleted<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.DELETED);
        }
        public static TypeQueryBuilder<T> GetId<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.ID);
        }
        public static TypeQueryBuilder<T> GetModified<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.MODIFIED);
        }
        public static TypeQueryBuilder<T> GetScore<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.SCORE);
        }
        public static TypeQueryBuilder<T> GetTypeName<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.TYPE_NAME);
        }
        public static TypeQueryBuilder<T> GetFullText<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.Field(FIELDS.FULLTEXT);
        }
        #endregion

        #region Find adaptation
        /// <summary>
        /// Using synonym for the full text search.
        /// </summary>
        /// <param name="synonyms">The slot of synonym you was config on the Graph server. It can be ONE or TWO or both.</param>
        public static TypeQueryBuilder<T> UsingSynonyms<T>(this TypeQueryBuilder<T> typeQueryBuilder, params Synonyms[] synonyms)
        {
            if (synonyms != null && synonyms.Length > 0)
            {
                return typeQueryBuilder.Where(FIELDS.FULLTEXT, new StringFilterOperators().Synonym(synonyms));
            }
            return typeQueryBuilder.Where(FIELDS.FULLTEXT, new StringFilterOperators().Synonym(Synonyms.ONE, Synonyms.TWO));
        }
        public static TypeQueryBuilder<T> Take<T>(this TypeQueryBuilder<T> typeQueryBuilder, int take)
        {
            return typeQueryBuilder.Limit(take);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, string fieldName, IFilterOperator filterOperator)
        {
            filterOperator.ValidateNotNullArgument("filterOperator");
            return typeQueryBuilder.Where(fieldName, filterOperator);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, Expression<Func<T, Filter>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            return typeQueryBuilder.Where(fieldSelector);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            return typeQueryBuilder.Where(fieldSelector, filterOperator);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, Expression<Func<T, long?>> fieldSelector, NumericFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            return typeQueryBuilder.Where(fieldSelector, filterOperator);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, Expression<Func<T, string>> fieldSelector, DateFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            return typeQueryBuilder.Where(fieldSelector, filterOperator);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, Expression<Func<T, bool>> fieldSelector, BooleanFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            return typeQueryBuilder.Where(fieldSelector, filterOperator);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, Expression<Func<T, DateTime?>> fieldSelector, DateFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            return typeQueryBuilder.Where(fieldSelector, filterOperator);
        }
        public static TypeQueryBuilder<T> Filter<T>(this TypeQueryBuilder<T> typeQueryBuilder, IFilter filter)
        {
            filter.ValidateNotNullArgument("filter");
            return typeQueryBuilder.Where(filter);
        }
        public static GraphQueryBuilder EndType<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.ToQuery();
        }
        #endregion

        #region Get results
        public static async Task<ContentGraphResult<TResult>> GetResultAsync<TResult>(this TypeQueryBuilder typeQueryBuilder)
        {
           return await typeQueryBuilder.ToQuery().BuildQueries().GetResultAsync<TResult>();
        }   
        public static async Task<ContentGraphResult> GetResultAsync(this TypeQueryBuilder typeQueryBuilder)
        {
           return await typeQueryBuilder.ToQuery().BuildQueries().GetResultAsync();
        }
        public static async Task<string> GetRawResultAsync(this TypeQueryBuilder typeQueryBuilder)
        {
            return await typeQueryBuilder.ToQuery().BuildQueries().GetRawResultAsync();
        }
        #endregion
    }
}
