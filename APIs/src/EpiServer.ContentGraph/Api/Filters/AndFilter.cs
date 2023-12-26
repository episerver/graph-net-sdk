using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class AndFilter<T> : AndFilter
    {
        #region constructors
        public AndFilter() : base()
        {
        }
        public AndFilter(string query) : base(query)
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
        public AndFilter(Expression<Func<T, DateTime?>> fieldSelector, DateFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        public AndFilter(Expression<Func<T, long?>> fieldSelector, NumericFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        public AndFilter(Expression<Func<T, int>> fieldSelector, NumericFilterOperators filterOperators)
        {
            And(fieldSelector, filterOperators);
        }
        #endregion
        public AndFilter<T> And(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, DateTime?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, long?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, double?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, int>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, bool>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        private AndFilter<T> And(Expression<Func<T, Filter>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var paser = new FilterExpressionParser();
            var filter = paser.GetFilter(fieldSelector);
            AddFilter(filter);
            return this;
        }
        public AndFilter<T> And(params Expression<Func<T, Filter>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var field in fieldSelectors)
            {
                And(field);
            }
            return this;
        }
    }

    public class AndFilter : Filter
    {
        string _query = string.Empty;
        public override string FilterClause
        {
            get
            {
                if (_filters != null && _filters.Count > 0)
                {
                    string otherFilters = string.Join(',', _filters.Select(x => $"{{{x.FilterClause}}}"));
                    if (_query.IsNullOrEmpty())
                    {
                        return $"_and:[{otherFilters}]";
                    }
                    return $"_and:[{_query},{otherFilters}]";
                }
                else
                {
                    return $"_and:[{_query}]";
                }
            }
        }
        #region constructors
        public AndFilter(string query)
        {
            _query = query;
        }
        public AndFilter() : base()
        {
        }
        #endregion
        public AndFilter And(string field, IFilterOperator filterOperator)
        {
            string filterClause = ConvertNestedFieldToString.ConvertNestedFieldFilter(field, filterOperator);
            if (!filterOperator.Query.IsNullOrEmpty())
            {
                if (_query.IsNullOrEmpty())
                {
                    _query = $"{{{filterClause}}}";
                }
                else
                {
                    _query += $",{{{filterClause}}}";
                }
            }
            return this;
        }
    }
}
