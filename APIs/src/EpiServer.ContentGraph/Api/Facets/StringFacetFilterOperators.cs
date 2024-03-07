using EPiServer.ContentGraph.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class StringFacetFilterOperators : IFacetOperator
    {
        string _query = string.Empty;
        IEnumerable<FacetProperty> _projections;
        public string FilterClause { get { return _query; } }
        public StringFacetFilterOperators()
        {
            _projections = new List<FacetProperty> { FacetProperty.name, FacetProperty.count };
        }
        public IEnumerable<FacetProperty> FacetProjections { get { return _projections; } }

        public StringFacetFilterOperators OrderBy(OrderMode orderMode = OrderMode.DESC)
        {
            _query = _query.IsNullOrEmpty() ? $"orderBy: {orderMode}" : $"{_query},orderBy: {orderMode}";
            return this;
        }
        public StringFacetFilterOperators Filters(params string[] values)
        {
            string combineValues = string.Join(',',values.Select(value => $"\"{value?.Trim()}\""));
            _query = _query.IsNullOrEmpty() ? $"filters: [{combineValues}]" : $"{_query},filters: [{combineValues}]";
            return this;
        }
        public StringFacetFilterOperators Limit(int limit=5)
        {
            _query = _query.IsNullOrEmpty() ? $"limit: {limit}" : $"{_query},limit: {limit}";
            return this;
        }
        public StringFacetFilterOperators OrderType(OrderType orderType=Api.OrderType.COUNT)
        {
            _query = _query.IsNullOrEmpty() ? $"orderType: {orderType}" : $"{_query},orderType: {orderType}";
            return this;
        }

        public IFacetOperator Projection(params FacetProperty[] facetProperties)
        {
            facetProperties.ValidateNotNullArgument("facetProperties");
            _projections = facetProperties.Distinct();
            return this;
        }
    }
}
