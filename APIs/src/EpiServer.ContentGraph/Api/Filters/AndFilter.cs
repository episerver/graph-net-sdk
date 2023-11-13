using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
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
            fieldSelector.Compile();
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, DateTime?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, long?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, double?>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, int>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            And(fieldSelector.GetFieldPath(), filterOperator);
            return this;
        }
        public AndFilter<T> And(Expression<Func<T, bool>> fieldSelector, IFilterOperator filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            And(fieldSelector.GetFieldPath(), filterOperator);
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
                if (Filters.IsNotNull() && Filters.Count > 0)
                {
                    string otherFilters = string.Join(',', Filters.Select(x => $"{{{x.FilterClause}}}"));
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
        public AndFilter()
        {
        }
        #endregion
        protected AndFilter And(string field, IFilterOperator filterOperator)
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
