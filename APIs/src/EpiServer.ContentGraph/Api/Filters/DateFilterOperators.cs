using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class DateFilterOperators : IFilterOperator
    {
        string _query = string.Empty;
        public string Query => _query;
        public DateFilterOperators Boost(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"boost: {value}" : $",boost: {value}";
            return this;
        }
        public DateFilterOperators Eq(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"eq: \"{value}\"" : $",eq: \"{value}\"";
            return this;
        }
        public DateFilterOperators NotEq(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"notEq: \"{value}\"" : $",notEq: \"{value}\"";
            return this;
        }
        public DateFilterOperators Gt(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"gt: {value}" : $",gt: {value}";
            return this;
        }
        public DateFilterOperators Gte(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"gte: {value}" : $",gte: {value}";
            return this;
        }
        public DateFilterOperators Lt(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"lt: {value}" : $",lt: {value}";
            return this;
        }
        public DateFilterOperators Lte(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"lte: {value}" : $",lte: {value}";
            return this;
        }
    }
}
