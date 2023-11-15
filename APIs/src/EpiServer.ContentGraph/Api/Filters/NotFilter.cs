using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
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
    }

    public class NotFilter : Filter
    {
        string _query = string.Empty;
        public NotFilter(string query)
        {
            _query = query;
        }
        public NotFilter()
        {
        }
        public override string FilterClause
        {
            get
            {
                if (Filters.IsNotNull() && Filters.Count > 0)
                {
                    string otherFilters = string.Join(',', Filters.Select(x => $"{{{x.FilterClause}}}"));
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
