using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Helpers.Linq
{
    /// <summary>
    /// A class used to search for <see cref="Expression"/> instances. 
    /// </summary>
    /// <typeparam name="TExpression">The type of <see cref="Expression"/> to search for.</typeparam>
    public class ExpressionFinder<TExpression> : ExpressionVisitor where TExpression : Expression
    {
        private readonly IList<TExpression> _result = new List<TExpression>();
        private Func<TExpression, bool> _predicate;

        /// <summary>
        /// Returns a list of <typeparamref name="TExpression"/> instances that matches the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> that represents the sub tree for which to start searching.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}"/> used to filter the result</param>
        /// <returns>A list of <see cref="Expression"/> instances that matches the given predicate.</returns>
        public IEnumerable<TExpression> Find(Expression expression, Func<TExpression, bool> predicate)
        {
            _result.Clear();
            _predicate = predicate;
            Visit(expression);
            return _result;
        }

        /// <summary>
        /// Visits each node of the <see cref="Expression"/> tree checks 
        /// if the current expression matches the predicate.         
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> currently being visited.</param>
        /// <returns><see cref="Expression"/></returns>
        protected override Expression Visit(Expression expression)
        {
            if (expression != null && expression is TExpression)
            {
                if (_predicate((TExpression)expression))
                    _result.Add((TExpression)expression);
            }

            return base.Visit(expression);
        }
    }
}
