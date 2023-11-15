using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Helpers.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public class FieldPathVisitor : EPiServer.ContentGraph.Helpers.Linq.ExpressionVisitor
    {
        private Expression fieldExpression;
        private StringBuilder path;
        private bool inGenericDictionaryKeyExpression;

        public FieldPathVisitor(Expression fieldExpression)
        {
            this.fieldExpression = fieldExpression;
        }

        public string GetPath()
        {
            path = new StringBuilder();
            Visit(fieldExpression);
            return path.ToString();
        }

        private void PrependMemberName(string name)
        {
            if (path.Length > 0)
            {
                path.Insert(0, ".");
            }
            path.Insert(0, name);
        }

        protected override Expression VisitMemberAccess(MemberExpression node)
        {
            if (!inGenericDictionaryKeyExpression)
            {
                PrependMemberName(node.Member.Name);
            }
            Expression exp = this.Visit(node.Expression);
            if (exp != node.Expression)
            {
                var methodCall = node.Expression as MethodCallExpression;
                if (methodCall != null && methodCall.IsGetItemInvokationOnGenericDictionary())
                {
                    return node;
                }
                return Expression.MakeMemberAccess(exp, node.Member);
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var handlingGenericDictionaryItemGetter = false;
            if (!inGenericDictionaryKeyExpression && node.IsGetItemInvokationOnGenericDictionary())
            {
                //We're dealing with an expression such as myDictionary["key"]
                //and want the member name to be "key" instead of "get_Item"
                handlingGenericDictionaryItemGetter = true;

                var key = node.Arguments[0].CachedCompileInvoke().ToString();
                PrependMemberName(key);
            }
            else if (!inGenericDictionaryKeyExpression)
            {
                PrependMemberName(node.Method.Name);
            }

            var returnValue = node;
            Expression obj = this.Visit(node.Object);

            if (handlingGenericDictionaryItemGetter)
            {
                //We need to prevent other visitors from writing to the member name
                //while visiting the key for the dictionary as we've already taken care of that.
                inGenericDictionaryKeyExpression = true;
            }
            IEnumerable<Expression> args = this.VisitExpressionList(node.Arguments);
            if (obj != node.Object || args != node.Arguments)
            {

                returnValue = Expression.Call(obj, node.Method, args);

            }
            if (handlingGenericDictionaryItemGetter)
            {
                inGenericDictionaryKeyExpression = false;
            }

            return returnValue;
        }
    }
}
