using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class NotFilter<T> : NotFilter
    {
        #region constructors
        public NotFilter(): base()
        {
        }
        public NotFilter(string query) : base(query)
        {
        }
        public NotFilter(Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter(Expression<Func<T, string>> fieldSelector, DateFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }       
        public NotFilter(Expression<Func<T, DateTime?>> fieldSelector, DateFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter(Expression<Func<T, long?>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        #endregion
        public NotFilter<T> Not(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, long?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, double?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, float?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, DateTime?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, bool>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        private NotFilter<T> Not(Expression<Func<T, Filter>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var paser = new FilterExpressionParser();
            var filter = paser.GetFilter(fieldSelector);
            AddFilter(filter);
            return this;
        }
        public NotFilter<T> Not(params Expression<Func<T, Filter>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var field in fieldSelectors)
            {
                Not(field);
            }
            return this;
        }
        #region Enumerable
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<string>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter(Expression<Func<T, IEnumerable<string>>> fieldSelector, StringFilterOperators filterOperators)
        {
            Not(fieldSelector, filterOperators);
        }
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<bool>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<int>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<double>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<long>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<float>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not(Expression<Func<T, IEnumerable<DateTime>>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public NotFilter<T> Not<TField>(Expression<Func<T, IEnumerable<TField>>> rootSelector,
        Expression<Func<TField, object>> fieldSelector,
        IFilterOperator filterOperator)
        {
            rootSelector.ValidateNotNullArgument("rootSelector");
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Not($"{rootSelector.GetFieldPath()}.{fieldSelector.GetFieldPath()}", filterOperator);
            return this;
        }
        #endregion
    }

    public class NotFilter : Filter
    {
        string _query = string.Empty;
        public NotFilter(string query)
        {
            _query = query;
        }
        public NotFilter() : base()
        {
        }
        public override string FilterClause
        {
            get
            {
                if (_filters != null && _filters.Count > 0)
                {
                    string otherFilters = string.Join(',', _filters.Select(x => $"{{{x.FilterClause}}}"));
                    if (_query.IsNullOrEmpty())
                    {
                        return $"_not:[{otherFilters}]";
                    }
                    return $"_not:[{_query},{otherFilters}]";
                }
                else
                {
                    return $"_not:[{_query}]";
                }
            }
        }
        public NotFilter Not(string field, IFilterOperator filterOperator)
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
