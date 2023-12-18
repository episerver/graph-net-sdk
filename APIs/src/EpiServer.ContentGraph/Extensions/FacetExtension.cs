using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.ExpressionHelper;

namespace EPiServer.ContentGraph.Extensions
{
    public static class FacetExtension
    {
        public static DelegateFacetFilterBuilder FacetLimit(this object field, int limit=5)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperator().Limit(limit)));
        }
        public static DelegateFacetFilterBuilder FacetFilters(this string field, params string[] values)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperator().Filters(values)));
        }
        public static DelegateFacetFilterBuilder FacetOrder(this string field, OrderType orderType, OrderMode orderMode = OrderMode.ASC)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperator().OrderType(orderType).OrderBy(orderMode)));
        }
        public static DelegateFacetFilterBuilder FacetOrder(this bool field, OrderType orderType, OrderMode orderMode = OrderMode.ASC)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new StringFacetFilterOperator().OrderType(orderType).OrderBy(orderMode)));
        }
        public static DelegateFacetFilterBuilder FacetUnit(this DateTime field, DateUnit dateUnit)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new DateFacetFilterOperator().Unit(dateUnit)));
        }
        public static DelegateFacetFilterBuilder FacetUnit(this DateTime field, int value)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new DateFacetFilterOperator().Value(value)));
        }
        public static DelegateFacetFilterBuilder FacetInRange(this int field, int? from, int? to)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new NumericFacetFilterOperator().Ranges((from, to))));
        }
        public static DelegateFacetFilterBuilder FacetInRange(this float field, float? from, float? to)
        {
            return new DelegateFacetFilterBuilder(field => new TermFacetFilter(field, new NumericFacetFilterOperator().Ranges((from, to))));
        }
    }
}
