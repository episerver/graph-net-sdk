using EPiServer.ContentGraph.Helpers;
using System.Linq;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class StringFilterOperators : IFilterOperator
    {
        string _query = string.Empty;
        public string Query => _query;
        public static StringFilterOperators Create()
        {
            return new StringFilterOperators();
        }
        /// <summary>
        /// Only for searchable field
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
            _query += _query.IsNullOrEmpty() ? $"exist: {value.ToString().ToLower()}" : $",exist: {value.ToString().ToLower()}";
            return this;
        }
        public StringFilterOperators In(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"in: \"{value}\"" : $",in: \"{value}\"";
            return this;
        }
        public StringFilterOperators In(params string[] values)
        {
            values.ValidateNotNullOrEmptyArgument("values");
            values = values.Select(x => $"\"{x}\"").ToArray();
            _query += _query.IsNullOrEmpty() ? $"in: [{string.Join(',',values)}]" : $",in: [{string.Join(',', values)}]";
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
        public StringFilterOperators NotIn(params string[] values)
        {
            values.ValidateNotNullOrEmptyArgument("values");
            values = values.Select(x => $"\"{x}\"").ToArray();
            _query += _query.IsNullOrEmpty() ? $"notIn: [{string.Join(',', values)}]" : $",notIn: [{string.Join(',', values)}]";
            return this;
        }
        public StringFilterOperators StartWith(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"startsWith: \"{value}\"" : $",startsWith: \"{value}\"";
            return this;
        }
        /// <summary>
        /// Not use for searchable field
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public StringFilterOperators EndWith(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"endsWith: \"{value}\"" : $",endsWith: \"{value}\"";
            return this;
        }
        public StringFilterOperators Synonym(params Synonyms[] values)
        {
            string synonymValues;
            if (values != null && values.Length > 0)
            {
                synonymValues = string.Join(',', values);
            }
            else
            {
                synonymValues = "ONE,TWO";
            }
            _query += _query.IsNullOrEmpty() ? $"synonyms: [{synonymValues}]" : $",synonyms: [{synonymValues}]";
            return this;
        }
        public StringFilterOperators Fuzzy(bool value)
        {
            _query += _query.IsNullOrEmpty() ? $"fuzzy: {value.ToString().ToLower()}" : $",fuzzy: {value.ToString().ToLower()}";
            return this;
        }
        /// <summary>
        /// Only for searchable field
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public StringFilterOperators Match(string value)
        {
            _query += _query.IsNullOrEmpty() ? $"match: \"{value}\"" : $",match: \"{value}\"";
            return this;
        }
    }
}
