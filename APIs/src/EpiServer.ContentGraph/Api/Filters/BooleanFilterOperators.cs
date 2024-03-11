using EPiServer.ContentGraph.Helpers;
namespace EPiServer.ContentGraph.Api.Filters
{
    public class BooleanFilterOperators : IFilterOperator
    {
        string _query = string.Empty;
        public static BooleanFilterOperators Create()
        {
            return new BooleanFilterOperators();
        }
        public string Query => _query;

        public BooleanFilterOperators Boost(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"boost: {value}" : $",boost: {value}";
            return this;
        }
        public BooleanFilterOperators Exists(bool value)
        {
            _query += _query.IsNullOrEmpty() ? $"exist: {value.ToString().ToLower()}" : $",exist: {value.ToString().ToLower()}";
            return this;
        }
        public BooleanFilterOperators Eq(bool value)
        {
            _query += _query.IsNullOrEmpty() ? $"eq: {value.ToString().ToLower()}" : $",eq: {value.ToString().ToLower()}";
            return this;
        }
        public BooleanFilterOperators NotEq(bool value)
        {
            _query += _query.IsNullOrEmpty() ? $"notEq: {value.ToString().ToLower()}" : $",notEq: {value.ToString().ToLower()}";
            return this;
        }
    }
}
