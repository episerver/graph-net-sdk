using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class StringFilterOperators : IFilterOperator
    {
        string _query;
        public string Query => _query;

        public StringFilterOperators Contains(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"contains: \"{value}\"" : $",contains: \"{value}\"";
            return this;
        }
        public StringFilterOperators Boost(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"boost: {value}" : $",boost: {value}";
            return this;
        }
        public StringFilterOperators Eq(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"eq: \"{value}\"" : $",eq: \"{value}\"";
            return this;
        }
        public StringFilterOperators Exists(bool value)
        {
            _query += _query.IsNullOrEmpty() ? $"exist: {value}" : $",exist: {value}";
            return this;
        }
        public StringFilterOperators In(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"in: \"{value}\"" : $",in: \"{value}\"";
            return this;
        }
        public StringFilterOperators Like(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"like: \"{value}\"" : $",like: \"{value}\"";
            return this;
        }
        public StringFilterOperators NotEq(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"notEq: \"{value}\"" : $",notEq: \"{value}\"";
            return this;
        }
        public StringFilterOperators NotIn(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"notIn: \"{value}\"" : $",notIn: \"{value}\"";
            return this;
        }
        public StringFilterOperators StartWith(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"startsWith: \"{value}\"" : $",startsWith: \"{value}\"";
            return this;
        }
        public StringFilterOperators EndWith(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"endsWith: \"{value}\"" : $",endsWith: \"{value}\"";
            return this;
        }
        public StringFilterOperators Synonym(Synonyms value)
        {
            _query += _query.IsNullOrEmpty() ? $"synonyms: {value}" : $",synonyms: {value}";
            return this;
        }
    }
}
