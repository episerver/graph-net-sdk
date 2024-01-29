using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers;
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
        public static TypeQueryBuilder<T> UsingSynonym<T>(this TypeQueryBuilder<T> typeQueryBuilder, params Api.Synonyms[] synonyms)
        {
            if (synonyms != null)
            {
                return typeQueryBuilder.Where("_fulltext", new StringFilterOperators().Synonym(synonyms));
            }
            return typeQueryBuilder.Where("_fulltext", new StringFilterOperators().Synonym(Api.Synonyms.ONE, Api.Synonyms.TWO));
        }
        #endregion

        #region Find adaptation
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
            filter.ValidateNotNullArgument("booleanFilter");
            return typeQueryBuilder.Where(filter);
        }
        public static GraphQueryBuilder EndType<T>(this TypeQueryBuilder<T> typeQueryBuilder)
        {
            return typeQueryBuilder.ToQuery();
        }
        #endregion
    }
}
