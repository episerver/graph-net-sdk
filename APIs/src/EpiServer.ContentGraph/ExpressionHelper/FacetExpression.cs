using EPiServer.ContentGraph.Api.Facets;
using System;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    internal class FacetExpression<T> : FacetFilter
    {
        internal static FacetFilter ParseFilterExpression<TSource>(Expression<Func<TSource, FacetFilter>> filterExpression)
        {
            return new FacetExpressionParser().GetFacetFilter(filterExpression);
        }

        public FacetExpression(Expression<Func<T, FacetFilter>> expression):base(string.Empty)
        {
            this.Expression = expression;
        }

        public Expression<Func<T, FacetFilter>> Expression { get; set; }

        public override string FilterClause => string.Empty;

    }
}
