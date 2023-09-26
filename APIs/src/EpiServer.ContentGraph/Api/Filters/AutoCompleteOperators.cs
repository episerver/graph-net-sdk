using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Autocomplete
{
    public class AutoCompleteOperators : IFilterOperator
    {
        string _query = string.Empty;

        public string Query => _query;

        public AutoCompleteOperators Value(string value)
        {
            _query = _query.IsNullOrEmpty() ? $"value:\"{value}\"" : string.Join(_query, $",value:\"{value}\"");
            return this;
        }
        public AutoCompleteOperators Limit(int limit)
        {
            _query = _query.IsNullOrEmpty() ? 
                $"limit:{limit}" : 
                string.Concat(_query, $",limit:{limit}");
            return this;
        }
    }
}
