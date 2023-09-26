using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class OrFilter<T> : IGraphFilter
    {
        string _query = string.Empty;
        public OrFilter(string query)
        {
            _query = query;
        }
        public OrFilter()
        {
        }
        public string FilterClause => $"_or:{{{_query}}}";

        public OrFilter<T> Or(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
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
