using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Facets;

namespace EPiServer.ContentGraph.Extensions
{
    public static class FacetExtension
    {
        public static DelegateFacetFilterBuilder FacetLimit(this object field, int limit=5)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().Limit(limit)));
        }
        public static DelegateFacetFilterBuilder FacetFilters(this string field, params string[] values)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().Filters(values)));
        }

        public static DelegateFacetFilterBuilder FacetFilters(this IEnumerable<string> field, params string[] values)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().Filters(values)));
        }
        public static DelegateFacetFilterBuilder FacetOrder(this string field, OrderType orderType, OrderMode orderMode = OrderMode.ASC)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().OrderType(orderType).OrderBy(orderMode)));
        }
        public static DelegateFacetFilterBuilder FacetOrder(this IEnumerable<string> field, OrderType orderType, OrderMode orderMode = OrderMode.ASC)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().OrderType(orderType).OrderBy(orderMode)));
        }
        public static DelegateFacetFilterBuilder FacetOrder(this bool field, OrderType orderType, OrderMode orderMode = OrderMode.ASC)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().OrderType(orderType).OrderBy(orderMode)));
        }
        public static DelegateFacetFilterBuilder FacetOrder(this IEnumerable<bool> field, OrderType orderType, OrderMode orderMode = OrderMode.ASC)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperators().OrderType(orderType).OrderBy(orderMode)));
        }
        public static DelegateFacetFilterBuilder FacetUnit(this DateTime field, DateUnit dateUnit)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new DateFacetFilterOperators().Unit(dateUnit)));
        }
        public static DelegateFacetFilterBuilder FacetUnit(this IEnumerable<DateTime> field, DateUnit dateUnit)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new DateFacetFilterOperators().Unit(dateUnit)));
        }
        public static DelegateFacetFilterBuilder FacetUnit(this DateTime field, int value)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new DateFacetFilterOperators().Value(value)));
        }
        public static DelegateFacetFilterBuilder FacetUnit(this IEnumerable<DateTime> field, int value)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new DateFacetFilterOperators().Value(value)));
        }
        public static DelegateFacetFilterBuilder FacetInRange(this int field, int? from, int? to)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new NumericFacetFilterOperators().Ranges((from, to))));
        }
        public static DelegateFacetFilterBuilder FacetInRange(this IEnumerable<int> field, int? from, int? to)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new NumericFacetFilterOperators().Ranges((from, to))));
        }
        public static DelegateFacetFilterBuilder FacetInRange(this float field, float? from, float? to)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new NumericFacetFilterOperators().Ranges((from, to))));
        }
        public static DelegateFacetFilterBuilder FacetInRange(this IEnumerable<float> field, float? from, float? to)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new NumericFacetFilterOperators().Ranges((from, to))));
        }
    }
}
