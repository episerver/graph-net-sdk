using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class FilterOperator
    {
        public static FilterOperator INSTANCE { get { return new FilterOperator(); } }
        string query = string.Empty;
        public string Query { get { return query; } }
        public FilterOperator Contains(string value)
        {
            query += query.IsNullOrEmpty() ? $"contains: \"{value}\"" : $",contains: \"{value}\"";
            return this;
        }
        public FilterOperator Boost(int value)
        {
            query += query.IsNullOrEmpty() ? $"boost: {value}" : $",boost: {value}";
            return this;
        }
        public FilterOperator Eq(string value)
        {
            query += query.IsNullOrEmpty() ? $"eq: \"{value}\"" : $",eq: \"{value}\"";
            return this;
        }
        public FilterOperator Exists(bool value)
        {
            query += query.IsNullOrEmpty() ? $"exist: {value}" : $",exist: {value}";
            return this;
        }
        public FilterOperator In(string value)
        {
            query += query.IsNullOrEmpty() ? $"in: \"{value}\"" : $",in: \"{value}\"";
            return this;
        }
        public FilterOperator Like(string value)
        {
            query += query.IsNullOrEmpty() ? $"like: \"{value}\"" : $",like: \"{value}\"";
            return this;
        }
        public FilterOperator NotEq(string value)
        {
            query += query.IsNullOrEmpty() ? $"notEq: \"{value}\"" : $",notEq: \"{value}\"";
            return this;
        }
        public FilterOperator NotIn(string value)
        {
            query += query.IsNullOrEmpty() ? $"notIn: \"{value}\"" : $",notIn: \"{value}\"";
            return this;
        }
        public FilterOperator StartWith(string value)
        {
            query += query.IsNullOrEmpty() ? $"startsWith: \"{value}\"" : $",startsWith: \"{value}\"";
            return this;
        }
        public FilterOperator EndWith(string value)
        {
            query += query.IsNullOrEmpty() ? $"endsWith: \"{value}\"" : $",endsWith: \"{value}\"";
            return this;
        }
        public FilterOperator Synonym(Synonyms value)
        {
            query += query.IsNullOrEmpty() ? $"synonyms: {value}" : $",synonyms: {value}";
            return this;
        }
        public FilterOperator Gt(int value)
        {
            query += query.IsNullOrEmpty() ? $"gt: {value}" : $",gt: {value}";
            return this;
        }
        public FilterOperator Gte(int value)
        {
            query += query.IsNullOrEmpty() ? $"gte: {value}" : $",gte: {value}";
            return this;
        }
        public FilterOperator Lt(int value)
        {
            query += query.IsNullOrEmpty() ? $"lt: {value}" : $",lt: {value}";
            return this;
        }
        public FilterOperator Lte(int value)
        {
            query += query.IsNullOrEmpty() ? $"lte: {value}" : $",lte: {value}";
            return this;
        }
        public FilterOperator Gt(string value)
        {
            query += query.IsNullOrEmpty() ? $"gt: \"{value}\"" : $",gt:  \"{value}\"";
            return this;
        }
        public FilterOperator Gte(string value)
        {
            query += query.IsNullOrEmpty() ? $"gte:  \"{value}\"" : $",gte:  \"{value}\"";
            return this;
        }
        public FilterOperator Lt(string value)
        {
            query += query.IsNullOrEmpty() ? $"lt:  \"{value}\"" : $",lt:  \"{value}\"";
            return this;
        }
        public FilterOperator Lte(string value)
        {
            query += query.IsNullOrEmpty() ? $"lte:  \"{value}\"" : $",lte:  \"{value}\"";
            return this;
        }
        public enum Synonyms
        {
            ONE,
            TWO
        }
    }
}
