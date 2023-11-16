using EPiServer.ContentGraph.Api.Facets;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    internal class FacetExpression<T> : IFacetFilter
    {
        internal static IFacetFilter ParseFilterExpression<TSource>(Expression<Func<TSource, IFacetFilter>> filterExpression)
        {
            return new FacetExpressionParser().GetFacetFilter(filterExpression);
        }

        public FacetExpression(Expression<Func<T, IFacetFilter>> expression)
        {
            this.Expression = expression;
        }

        public Expression<Func<T, IFacetFilter>> Expression { get; set; }

        public string FilterClause => throw new NotImplementedException();

    }
}
