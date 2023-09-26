using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Helpers.Linq
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns a list of <typeparamref name="TExpression"/> instances
        /// that matches the <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TExpression">The type of <see cref="Expression"/>
        /// to search for.</typeparam>
        /// <param name="expression">The <see cref="Expression"/> that represents the sub tree for which to start searching.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}"/> used to filter the result</param>
        /// <returns>A list of <see cref="Expression"/> instances that matches the given predicate.</returns>
        public static IEnumerable<TExpression> Find<TExpression>(this Expression expression, Func<TExpression, bool> predicate) where TExpression : Expression
        {
            var finder = new ExpressionFinder<TExpression>();
            return finder.Find(expression, predicate);
        }

        /// <summary>
        /// Searches for expressions using the given <paramref name="predicate"/> and replaces matches with
        /// the result from the <paramref name="replaceWith"/> delegate.
        /// </summary>
        /// <typeparam name="TExpression">The type of <see cref="Expression"/> to search for.</typeparam>
        /// <param name="expression">The <see cref="Expression"/> that represents the sub tree
        /// for which to start searching.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}"/> used to filter the result</param>
        /// <param name="replaceWith">The <see cref="Func{T,TResult}"/> used to specify the replacement expression.</param>
        /// <returns>The modified <see cref="Expression"/> tree.</returns>
        public static Expression Replace<TExpression>(this Expression expression, Func<TExpression, bool> predicate, Func<TExpression, Expression> replaceWith) where TExpression : Expression
        {

            var replacer = new ExpressionReplacer<TExpression>();

            return replacer.Replace(expression, predicate, replaceWith);

        }

        public static bool IsGetItemInvokationOnGenericDictionary(this MethodCallExpression methodCall)
        {
            if (methodCall == null || methodCall.Object == null)
            {
                return false;
            }

            var invokationTargetType = methodCall.Object.Type;
            if (!invokationTargetType.IsGenericType)
            {
                return false;
            }

            var typeArguments = invokationTargetType.GetGenericArguments();
            if (typeArguments.Length != 2)
            {
                return false;
            }

            var dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeArguments);
            if (!dictionaryType.IsAssignableFrom(invokationTargetType))
            {
                return false;
            }
            if (methodCall.Method.Name != "get_Item")
            {
                return false;
            }

            return true;
        }
    }
}
