using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class AndFilter<T> : IGraphFilter
    {
        string _query = string.Empty;
        public AndFilter(string query)
        {
            _query = query;
        }
        public AndFilter()
        {
        }
        public string FilterClause => $"_and:{{{_query}}}";

        public AndFilter<T> And(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            string fieldName = fieldSelector.GetFieldPath();
            if (!filterOperator.Query.IsNullOrEmpty())
            {
                if (_query.IsNullOrEmpty())
                {
                    _query = $"{fieldName}:{{{filterOperator.Query}}}";
                }
                else
                {
                    _query += $",{fieldName}:{{{filterOperator.Query}}}";
                }
            }
            return this;
        }
    }
}
