using EPiServer.ContentGraph.Api.Filters;
using System;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    internal class FilterExpression<T> : Filter
    {
        internal static Filter ParseFilterExpression<TSource>(Expression<Func<TSource, Filter>> filterExpression)
        {
            return new FilterExpressionParser().GetFilter(filterExpression);
        }

        public FilterExpression(Expression<Func<T, Filter>> expression)
        {
            this.Expression = expression;
        }

        public Expression<Func<T, Filter>> Expression { get; set; }
    }
}
