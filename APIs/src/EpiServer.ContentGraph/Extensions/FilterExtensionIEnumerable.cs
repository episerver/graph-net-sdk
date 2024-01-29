using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Filters;
using System;

namespace EPiServer.ContentGraph.Extensions
{
    public static partial class FilterExtension
    {
        #region String operators
        public static DelegateFilterBuilder Eq(this IEnumerable<string> field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this IEnumerable<string> field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder NotIn(this IEnumerable<string> field, string[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().NotIn(values)));
        }
        public static DelegateFilterBuilder In(this IEnumerable<string> field, string[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder Like(this IEnumerable<string> field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Like(value)));
        }
        public static DelegateFilterBuilder MatchPrefix(this IEnumerable<string> field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().StartWith(value)));
        }
        public static DelegateFilterBuilder MatchSuffix(this IEnumerable<string> field, string value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().EndWith(value)));
        }
        public static DelegateFilterBuilder MatchFuzzy(this IEnumerable<string> field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Fuzzy(value)));
        }
        public static DelegateFilterBuilder FieldExists(this IEnumerable<string> field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Synonyms(this IEnumerable<string> field, params Synonyms[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new StringFilterOperators().Synonym(values)));
        }
        #endregion

        #region Date operators
        public static DelegateFilterBuilder InRange(this IEnumerable<DateTime> field, DateTime from, DateTime to)
        {
            return new DelegateFilterBuilder(field =>
            new TermFilter(field, new DateFilterOperators()
                .Gte(from.ToString("s") + "Z")
                .Lt(to.ToString("s") + "Z")
            ));
        }
        public static DelegateFilterBuilder Eq(this IEnumerable<DateTime> field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Eq(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder NotEq(this IEnumerable<DateTime> field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().NotEq(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Gt(this IEnumerable<DateTime> field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Gt(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Gte(this IEnumerable<DateTime> field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Gte(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Lt(this IEnumerable<DateTime> field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Lt(value.ToString("s") + "Z")));
        }
        public static DelegateFilterBuilder Lte(this IEnumerable<DateTime> field, DateTime value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new DateFilterOperators().Lte(value.ToString("s") + "Z")));
        }
        #endregion

        #region Numberic operator
        //Integer type
        public static DelegateFilterBuilder FieldExists(this IEnumerable<int> field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Eq(this IEnumerable<int> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this IEnumerable<int> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder Gt(this IEnumerable<int> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gt(value)));
        }
        public static DelegateFilterBuilder Gte(this IEnumerable<int> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(value)));
        }
        public static DelegateFilterBuilder Lt(this IEnumerable<int> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lt(value)));
        }
        public static DelegateFilterBuilder Lte(this IEnumerable<int> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lte(value)));
        }
        public static DelegateFilterBuilder In(this IEnumerable<int> field, params int[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder NotIn(this IEnumerable<int> field, int[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotIn(values)));
        }
        /// <summary>
        /// Range filter for a field. The range is between greater than or equals [From] and less than or equals [To]
        /// </summary>
        public static DelegateFilterBuilder InRange(this IEnumerable<int> field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRange(from, to)));
        }
        /// <summary>
        /// Multiple ranges for InRange filter. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="ranges">Array of tuples (from,to)</param>
        /// <returns></returns>
        public static DelegateFilterBuilder InRanges(this IEnumerable<int> field, params (int? from, int? to)[] ranges)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRanges(ranges)));
        }
        //Float type
        public static DelegateFilterBuilder FieldExists(this IEnumerable<float> field, bool value = true)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Exists(value)));
        }
        public static DelegateFilterBuilder Boost(this IEnumerable<float> field, int value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Boost(value)));
        }
        public static DelegateFilterBuilder Eq(this IEnumerable<float> field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Eq(value)));
        }
        public static DelegateFilterBuilder NotEq(this IEnumerable<float> field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotEq(value)));
        }
        public static DelegateFilterBuilder Gt(this IEnumerable<float> field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gt(value)));
        }
        public static DelegateFilterBuilder Gte(this IEnumerable<float> field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Gte(value)));
        }
        public static DelegateFilterBuilder Lt(this IEnumerable<float> field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lt(value)));
        }
        public static DelegateFilterBuilder Lte(this IEnumerable<float> field, float value)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().Lte(value)));
        }
        public static DelegateFilterBuilder In(this IEnumerable<float> field, params float[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().In(values)));
        }
        public static DelegateFilterBuilder NotIn(this IEnumerable<float> field, float[] values)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().NotIn(values)));
        }
        /// <summary>
        /// Range filter for a field. The range is between greater than or equals [From] and less than or equals [To]
        /// </summary>
        public static DelegateFilterBuilder InRange(this IEnumerable<float> field, int from, int to)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRange(from, to)));
        }
        /// <summary>
        /// Multiple ranges for InRange filter. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="ranges">Array of tuples (from,to)</param>
        /// <returns></returns>
        public static DelegateFilterBuilder InRanges(this IEnumerable<float> field, params (float? from, float? to)[] ranges)
        {
            return new DelegateFilterBuilder(field => new TermFilter(field, new NumericFilterOperators().InRanges(ranges)));
        }

        #endregion
    }
}
