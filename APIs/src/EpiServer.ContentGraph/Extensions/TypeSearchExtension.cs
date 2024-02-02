using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Extensions
{
    public static class TypeSearchExtension
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

        #region Find adaptation
        /// <summary>
        /// Using synonym for your filter.
        /// </summary>
        /// <param name="synonyms">The slot of synonym you was config on the Graph server. It can be ONE or TWO or both.</param>
        public static TypeQueryBuilder<T> UsingSynonym<T>(this TypeQueryBuilder<T> typeQueryBuilder, params Synonyms[] synonyms)
        {
            if (synonyms != null && synonyms.Length > 0)
            {
                return typeQueryBuilder.Where("_fulltext", new StringFilterOperators().Synonym(synonyms));
            }
            return typeQueryBuilder.Where("_fulltext", new StringFilterOperators().Synonym(Synonyms.ONE, Synonyms.TWO));
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
    }
}
