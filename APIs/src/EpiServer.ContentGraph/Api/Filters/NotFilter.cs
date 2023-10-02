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
        public NotFilter(Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter(Expression<Func<T, string>> fieldSelector, DateFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter(Expression<Func<T, long>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter(Expression<Func<T, int>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter<T> Not(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
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
        public NotFilter<T> Not(Expression<Func<T, long>> fieldSelector, IFilterOperator filterOperator)
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
        public NotFilter<T> Not(Expression<Func<T, int>> fieldSelector, IFilterOperator filterOperator)
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
