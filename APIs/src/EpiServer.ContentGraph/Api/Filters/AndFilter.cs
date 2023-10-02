using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class AndFilter<T> : IGraphFilter
    {
        string _query = string.Empty;
        public string FilterClause => $"_and:{{{_query}}}";
        public AndFilter(string query)
        {
            _query = query;
        }
        public AndFilter()
        {
        }
        public AndFilter(Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        public AndFilter(Expression<Func<T, string>> fieldSelector, DateFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        public AndFilter(Expression<Func<T, long>> fieldSelector, NumericFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        public AndFilter(Expression<Func<T, int>> fieldSelector, NumericFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        public AndFilter<T> And(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
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
        public AndFilter<T> And(Expression<Func<T, long>> fieldSelector, IFilterOperator filterOperator)
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
        public AndFilter<T> And(Expression<Func<T, int>> fieldSelector, IFilterOperator filterOperator)
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
