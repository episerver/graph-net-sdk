using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class StringFacetFilterOperator : IFacetOperator
    {
        string _query = string.Empty;
        IEnumerable<FacetProperty> _projections;
        public string FilterClause { get { return _query; } }
        public StringFacetFilterOperator()
        {
            _projections = new List<FacetProperty> { FacetProperty.name, FacetProperty.count };
        }
        public IEnumerable<FacetProperty> FacetProjections { get { return _projections; } }

        public StringFacetFilterOperator OrderBy(OrderMode orderMode)
        {
            _query = _query.IsNullOrEmpty() ? $"orderBy: {orderMode}" : $"{_query},orderBy: {orderMode}";
            return this;
        }
        public StringFacetFilterOperator Filters(params string[] values)
        {
            string combineValues = string.Join(',',values.Select(value => $"\"{value}\""));
            _query = _query.IsNullOrEmpty() ? $"filters: [{combineValues}]" : $"{_query},filters: [{combineValues}]";
            return this;
        }
        public StringFacetFilterOperator Limit(int limit)
        {
            _query = _query.IsNullOrEmpty() ? $"limit: {limit}" : $"{_query},limit: {limit}";
            return this;
        }
        public StringFacetFilterOperator OrderType(OrderType orderType)
        {
            _query = _query.IsNullOrEmpty() ? $"orderType: {orderType}" : $"{_query},orderType: {orderType}";
            return this;
        }
        public StringFacetFilterOperator Projection(params FacetProperty[] projections)
        {
            projections.ValidateNotNullArgument("projections");
            _projections = projections.Distinct();
            return this;
        }
    }
}
