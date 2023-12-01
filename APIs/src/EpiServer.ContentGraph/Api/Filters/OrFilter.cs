using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class OrFilter<T> : OrFilter
    {
        #region constructors
        public OrFilter(): base()
        {
        }
        public OrFilter(string query) : base(query)
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
        public OrFilter(Expression<Func<T, long?>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Or(fieldSelector, filterOperators);
        }
        public OrFilter(Expression<Func<T, int>> fieldSelector, NumericFilterOperators filterOperators)
        {
            Or(fieldSelector, filterOperators);
        }
        #endregion
        public OrFilter<T> Or(Expression<Func<T, string>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Or(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, DateTime>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Or(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, long?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Or(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, double?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Or(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, int>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Or(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public OrFilter<T> Or(Expression<Func<T, bool>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            Or(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        private OrFilter<T> Or(Expression<Func<T, Filter>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var paser = new FilterExpressionParser();
            var filter = paser.GetFilter(fieldSelector);
            AddFilter(filter);
            return this;
        }
        public OrFilter<T> Or(params Expression<Func<T, Filter>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var field in fieldSelectors)
            {
                Or(field);
            }
            return this;
        }
    }

    public class OrFilter : Filter
    {
        string _query = string.Empty;
        public OrFilter(string query)
        {
            _query = query;
        }
        public OrFilter()
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
                        return $"_or:[{otherFilters}]";
                    }
                    return $"_or:[{_query},{otherFilters}]";
                }
                else
                {
                    return $"_or:[{_query}]";
                }
            }
        }
        public OrFilter Or(string field, IFilterOperator filterOperator)
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
