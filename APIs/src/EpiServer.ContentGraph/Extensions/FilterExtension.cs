using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Filters;

namespace EPiServer.ContentGraph.Extensions
{
    public static class FilterExtension
    {
        #region String operator
        /// <summary>
        /// Full text search. Only use for <typeparamref name="Searchable"/> field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns>DelegateFilterBuilder</returns>
        public static DelegateFilterBuilder Match(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Match(value)));
        }
        /// <summary>
        /// Full text search. Only use for <typeparamref name="Searchable"/> field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DelegateFilterBuilder ContainsValue(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Contains(value)));
        }
        public static DelegateFilterBuilder Eq(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder NotIn(this string field, string[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().NotIn(values)));
        }
        public static DelegateFilterBuilder In(this string field, string[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder Like(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Like(value)));
        }
        public static DelegateFilterBuilder MatchPrefix(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().StartWith(value)));
        }
        public static DelegateFilterBuilder MatchSuffix(this string field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().EndWith(value)));
        }
        public static DelegateFilterBuilder MatchFuzzy(this string field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Fuzzy(value)));
        }
        public static DelegateFilterBuilder FieldExists(this string field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Synonyms(this string field, params Synonyms[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Synonym(values)));
        }
        #endregion

        #region Date operator
        public static DelegateFilterBuilder InRange(this DateTime field, DateTime from, DateTime to)
        {
            return new DelegateFilterBuilder(field =>
            new TermFilter(field, new DateFilterOperators()
                .Gte(from.ToString("s") + "Z")
                .Lt(to.ToString("s") + "Z")
            ));
        }
        public static DelegateFilterBuilder Eq(this DateTime field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Eq(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder NotEq(this DateTime field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().NotEq(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Gt(this DateTime field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Gt(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Gte(this DateTime field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Gte(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Lt(this DateTime field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Lt(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Lte(this DateTime field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Lte(value.ToString("s") + "Z")));
        }
        #endregion

        #region Numberic operator
        public static DelegateFilterBuilder Eq(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder Gt(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gt(value)));
        }
        public static DelegateFilterBuilder Gte(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(value)));
        }
        public static DelegateFilterBuilder Lt(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lt(value)));
        }
        public static DelegateFilterBuilder Lte(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lte(value)));
        }
        public static DelegateFilterBuilder In(this int field, params int[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder NotIn(this int field, int[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotIn(values)));
        }
        public static DelegateFilterBuilder InRange(this int field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(from).Lte(to)));
        }
        #endregion
    }
}
