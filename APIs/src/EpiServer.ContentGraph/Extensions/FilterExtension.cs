using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Filters;
using System;

namespace EPiServer.ContentGraph.Extensions
{
    public static partial class FilterExtension
    {
        #region String operator
        public static DelegateFilterBuilder Boost(this string field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder FieldExists(this string field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Exists(value)));
        }
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
        public static DelegateFilterBuilder Synonyms(this string field, params Synonyms[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Synonym(values)));
        }
        #endregion

        #region Date operator
        public static DelegateFilterBuilder Boost(this DateTime field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Boost(value)));
        }
        /// <summary>
        /// Range for datetime filter. The range is between greater than or equals [From] and less than [To].
        /// </summary>
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
        /// <summary>
        /// Filter your pages that have been modified in a period of time.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static DelegateFilterBuilder ModifiedIn(this object field, DateTime? from, DateTime? to)
        {
            return new DelegateFilterBuilder(field =>
            {
                var dateOperator = new DateFilterOperators();
                if (from.HasValue)
                {
                    dateOperator.Gte(from.Value.ToString("s") + "Z");
                }
                if (to.HasValue)
                {
                    dateOperator.Lte(to.Value.ToString("s") + "Z");
                }
                return new TermFilter("_modified", dateOperator);
            });
        }
        #endregion

        #region Numberic operator

        #region Integer type
        public static DelegateFilterBuilder Boost(this int field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder FieldExists(this int field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Exists(value)));
        }
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
        /// <summary>
        /// Range filter for a field. The range is between greater than or equals [From] and less than or equals [To]
        /// </summary>
        public static DelegateFilterBuilder InRange(this int field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRange(from,to)));
        }
        /// <summary>
        /// Multiple ranges for InRange filter. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="ranges">Array of tuples (from,to)</param>
        /// <returns></returns>
        public static DelegateFilterBuilder InRanges(this int field, params (int? from, int? to)[] ranges)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRanges(ranges)));
        }
        #endregion

        #region Long type
        public static DelegateFilterBuilder Boost(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder FieldExists(this long field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Eq(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder Gt(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gt(value)));
        }
        public static DelegateFilterBuilder Gte(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(value)));
        }
        public static DelegateFilterBuilder Lt(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lt(value)));
        }
        public static DelegateFilterBuilder Lte(this long field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lte(value)));
        }
        public static DelegateFilterBuilder In(this long field, params int[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder NotIn(this long field, params int[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotIn(values)));
        }
        /// <summary>
        /// Range filter for a field. The range is between greater than or equals [From] and less than or equals [To]
        /// </summary>
        public static DelegateFilterBuilder InRange(this long field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRange(from, to)));
        }
        /// <summary>
        /// Multiple ranges for InRange filter. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="ranges">Array of tuples (from,to)</param>
        /// <returns></returns>
        public static DelegateFilterBuilder InRanges(this long field, params (int? from, int? to)[] ranges)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRanges(ranges)));
        }
        #endregion

        #region Float type
        public static DelegateFilterBuilder FieldExists(this float field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Boost(this float field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder Eq(this float field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this float field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder Gt(this float field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gt(value)));
        }
        public static DelegateFilterBuilder Gte(this float field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(value)));
        }
        public static DelegateFilterBuilder Lt(this float field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lt(value)));
        }
        public static DelegateFilterBuilder Lte(this float field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lte(value)));
        }
        public static DelegateFilterBuilder In(this float field, params float[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder NotIn(this float field, float[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotIn(values)));
        }
        /// <summary>
        /// Range filter for a field. The range is between greater than or equals [From] and less than or equals [To]
        /// </summary>
        public static DelegateFilterBuilder InRange(this float field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRange(from, to)));
        }
        /// <summary>
        /// Multiple ranges for InRange filter. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="ranges">Array of tuples (from,to)</param>
        /// <returns></returns>
        public static DelegateFilterBuilder InRanges(this float field, params (float? from, float? to)[] ranges)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRanges(ranges)));
        }
        #endregion

        #region Double type
        public static DelegateFilterBuilder FieldExists(this double field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Boost(this double field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder Eq(this double field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this double field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder Gt(this double field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gt(value)));
        }
        public static DelegateFilterBuilder Gte(this double field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(value)));
        }
        public static DelegateFilterBuilder Lt(this double field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lt(value)));
        }
        public static DelegateFilterBuilder Lte(this double field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lte(value)));
        }
        public static DelegateFilterBuilder In(this double field, params float[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder NotIn(this double field, params float[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotIn(values)));
        }
        /// <summary>
        /// Range filter for a field. The range is between greater than or equals [From] and less than or equals [To]
        /// </summary>
        public static DelegateFilterBuilder InRange(this double field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRange(from, to)));
        }
        /// <summary>
        /// Multiple ranges for InRange filter. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="ranges">Array of tuples (from,to)</param>
        /// <returns></returns>
        public static DelegateFilterBuilder InRanges(this double field, params (float? from, float? to)[] ranges)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRanges(ranges)));
        }
        #endregion

        #endregion

        #region Boolean operator
        public static DelegateFilterBuilder Boost(this bool field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new BooleanFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder FieldExists(this bool field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new BooleanFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Eq(this bool field, bool value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new BooleanFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this bool field, bool value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new BooleanFilterOperators().NotEq(value)));
        }
        #endregion
    }
}
