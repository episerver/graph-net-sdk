using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class StringFacetFilterOperator : IFacetFilterOperator
    {
        public StringFacetFilterOperator INSTANCE { get { return new StringFacetFilterOperator(); } }
        string _query = string.Empty;
        IEnumerable<FacetProjection> _projections;
        public string Query { get { return _query; } }

        public IEnumerable<FacetProjection> FacetProjections { get { return _projections; } }

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
        public StringFacetFilterOperator Projection(params FacetProjection[] projections)
        {
            projections.ValidateNotNullArgument("projections");
            _projections = projections.Distinct();
            return this;
        }
    }
}
