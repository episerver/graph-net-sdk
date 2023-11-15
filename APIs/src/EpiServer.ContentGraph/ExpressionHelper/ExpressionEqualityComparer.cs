using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Helpers;
using System.Linq.Expressions;


namespace EPiServer.ContentGraph.ExpressionHelper
{
    public class ExpressionEqualityComparer : IEqualityComparer<Expression>
    {
        private static Lazy<ExpressionHashCalculator> _expressionHashCalculator = new Lazy<ExpressionHashCalculator>(() => new ExpressionHashCalculator());

        public bool Equals(Expression expr1, Expression expr2)
        {
            if(expr1.IsNull() && expr2.IsNull())
            {
                return false;
            }

            if (object.ReferenceEquals(expr1, expr2))
            {
                return true;
            }

            return _expressionHashCalculator.Value.CalculateHashCode(expr1)  == _expressionHashCalculator.Value.CalculateHashCode(expr2);
        }

        public int GetHashCode(Expression obj)
        {
            if (obj.IsNull())
            {
                return -1;
            }

            return _expressionHashCalculator.Value.CalculateHashCode(obj).GetHashCode();
        }
    }
}