using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Autocomplete
{
    public class AutoCompleteOperators : IFilterOperator
    {
        public static AutoCompleteOperators Create()
        {
            return new AutoCompleteOperators();
        }
        string _query = string.Empty;

        public string Query => _query;

        public AutoCompleteOperators Value(string value)
        {
            _query = _query.IsNullOrEmpty() ? $"value:\"{value}\"" : $"{_query},value:\"{value}\"";
            return this;
        }
        public AutoCompleteOperators Limit(int limit = 25)
        {
            _query = _query.IsNullOrEmpty() ? $"limit:{limit}" : $"{_query},limit:{limit}";
            return this;
        }
    }
}
