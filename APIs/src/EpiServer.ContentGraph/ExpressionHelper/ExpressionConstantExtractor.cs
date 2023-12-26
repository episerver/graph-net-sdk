using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    public class ExpressionConstantExtractor : ExpressionVisitor
    {
        private readonly LambdaExpression _expression;
        private readonly List<object> _constants = new List<object>();
        private readonly ParameterExpression _constantsParameter = Expression.Parameter(typeof(object[]), "constants");

        public ExpressionConstantExtractor(LambdaExpression expression)
        {
            _expression = expression;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            // Add the constant value to list of constants and replace the constant node in the expression by: (NodeType)constants[numberOfConstantsFoundSoFar]

            _constants.Add(node.Value);

            return Expression.Convert(Expression.ArrayIndex(_constantsParameter, Expression.Constant(_constants.Count - 1)), node.Type);
        }

        public LambdaExpression ReplaceConstants(out object[] constants)
        {
            // should we explicitly create the new delegate type?  / Expression.GetDelegateType()


            var expression = Visit(_expression.Body);
            constants = _constants.ToArray();

            // Replace the root node by a lambda expression with the same body but with a new object[] constants parameter

            return Expression.Lambda(expression, new[] { _constantsParameter }.Union(_expression.Parameters));
        }
    }
}
