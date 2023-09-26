using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class FacetFilter
    {
        string _query = string.Empty;
        public string Query { get { return _query; } }
        public FacetFilter OrderBy(OrderMode orderMode)
        {
            _query = _query.IsNullOrEmpty() ? $"orderBy: {orderMode}" : $",orderBy: {orderMode}";
            return this;
        }
        public FacetFilter Filters(string value)
        {
            _query = _query.IsNullOrEmpty() ? $"filters: \"{value}\"" : $",filters: \"{value}\"";
            return this;
        }
        public FacetFilter Limit(int limit)
        {
            _query = _query.IsNullOrEmpty() ? $"limit: {limit}" : $",limit: {limit}";
            return this;
        }
        public FacetFilter OrderType(OrderType orderType)
        {
            _query = _query.IsNullOrEmpty() ? $"orderType: {orderType}" : $",orderType: {orderType}";
            return this;
        }
    }
}
