using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class NotFilter<T> : IGraphFilter
    {
        string _query = string.Empty;
        public NotFilter(string query)
        {
            _query = query;
        }
        public NotFilter()
        {
        }
        public string FilterClause => $"_not:{{{_query}}}";
        public NotFilter<T> Or(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
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
