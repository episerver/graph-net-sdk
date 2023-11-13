using EPiServer.ContentGraph.Api.Filters;

namespace EPiServer.ContentGraph.Extensions
{
    public static class FilterExtension
    {
        #region String operator
        /// <summary>
        /// Only use for <typeparamref name="Searchable"/> field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DelegateFilterBuilder Match(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new WrappedFilter(field, new StringFilterOperators().Contains(value)));
        }
        public static DelegateFilterBuilder Eq(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new WrappedFilter(field, new StringFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new WrappedFilter(field, new StringFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder NotIn(this string field, string[] values)
        {
            return new DelegateFilterBuilder(field => new WrappedFilter(field, new StringFilterOperators().NotIn(values)));
        }
        public static DelegateFilterBuilder In(this string field, string[] values)
        {
            return new DelegateFilterBuilder(field => new WrappedFilter(field, new StringFilterOperators().In(values)));
        }
        #endregion

        public static DelegateFilterBuilder InRange(this DateTime field, (DateTime from, DateTime to) ranges) 
        {
            return new DelegateFilterBuilder(field => 
            new WrappedFilter(field, new DateFilterOperators()
                .Gte(ranges.from.ToString("s") + "Z")
                .Lt(ranges.to.ToString("s") + "Z")
            ));
        }
    }
}
