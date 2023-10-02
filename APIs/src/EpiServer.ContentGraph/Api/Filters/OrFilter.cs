using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class OrFilter<T> : IGraphFilter
    {
        string _query = string.Empty;
        public string FilterClause => $"_or:{{{_query}}}";
        public OrFilter(string query)
        {
            _query = query;
        }
        public OrFilter()
        {
        }
        public OrFilter(Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperators)
        {
            Or(fieldSelector, filterOperators);
        }
        public OrFilter(Expression<Func<T, DateTime>> fieldSelector, DateFilterOperators filterOperators)
        {
            Or(fieldSelector, filterOperators);
        }
        public OrFilter(Expression<Func<T, long>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Or(fieldSelector, filterOperators);
        }
        public OrFilter(Expression<Func<T, int>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Or(fieldSelector, filterOperators);
        }
        public OrFilter<T> Or(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            string filterClause = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);
            if (!filterOperator.Query.IsNullOrEmpty())
            {
                if (_query.IsNullOrEmpty())
                {
                    _query = $"{filterClause}";
                }
                else
                {
                    _query += $",{filterClause}";
                }
            }
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, DateTime>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            string filterClause = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);
            if (!filterOperator.Query.IsNullOrEmpty())
            {
                if (_query.IsNullOrEmpty())
                {
                    _query = $"{filterClause}";
                }
                else
                {
                    _query += $",{filterClause}";
                }
            }
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, long>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            string filterClause = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);
            if (!filterOperator.Query.IsNullOrEmpty())
            {
                if (_query.IsNullOrEmpty())
                {
                    _query = $"{filterClause}";
                }
                else
                {
                    _query += $",{filterClause}";
                }
            }
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, int>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            string filterClause = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);
            if (!filterOperator.Query.IsNullOrEmpty())
            {
                if (_query.IsNullOrEmpty())
                {
                    _query = $"{filterClause}";
                }
                else
                {
                    _query += $",{filterClause}";
                }
            }
            return this;
        }
    }
}
