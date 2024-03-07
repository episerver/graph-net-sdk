using System.Linq.Expressions;
using System.Reflection;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    public class ExpressionHashCalculator
    {
        private static PropertyInfo _debugViewProperty = typeof(Expression).GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);

        public string CalculateHashCode(Expression obj)
        {
            return _debugViewProperty.GetValue(obj) as string;
        }
    }
}
