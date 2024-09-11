using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Linq;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    public class FragmentExpressionParser
    {
        public virtual Recursion GetReturnType<TSource>(Expression<Func<TSource, Recursion>> expression)
        {
            ValidateExpression(expression);

            Expression executable = expression.Body;

            //Find and replace methods returning DeligateRecursiveBuilder
            executable = executable.Replace<MethodCallExpression>(
                x => x.Method.ReturnType == typeof(DeligateRecursiveBuilder),
                x => Expression.Constant(GetFromDelegateBuilderMethod(x, null)));

            return executable.CachedCompileInvoke<Recursion>();
        }

        protected virtual void ValidateExpression<TSource>(Expression<Func<TSource, Recursion>> expression)
        {
            if (expression.Body.Find<NewExpression>(x => x.Type == typeof(DeligateRecursiveBuilder)).Count() > 0)
            {
                throw new NotSupportedException
                    (string.Format("Instantiating new {0} is not supported."
                                   + "The {0} class is intended to be used as a return value from extensions methods.",
                                   typeof(DeligateRecursiveBuilder).Name));
            }
            var methodsReturningFilterBuilder =
                expression.Body.Find<MethodCallExpression>(
                    x => !x.Method.IsExtensionMethod() && x.Method.ReturnType == typeof(DeligateRecursiveBuilder));
            if (methodsReturningFilterBuilder.Count() > 0)
            {
                throw new NotSupportedException(
                    string.Format(
                        "Method {0} returns {1}. Only extension methods should return {1}.",
                        methodsReturningFilterBuilder.First().Method.Name,
                        typeof(Recursion).Name));
            }
        }

        protected bool ReturnsExpression(MethodCallExpression x)
        {
            return x.Method.HasGenericTypeDefinition(typeof(Recursion));
        }

        protected Expression RealizeExpressionMethodCalls(MethodCallExpression methodCall)
        {
            var parsed = GetExpressionFromExpressionMethod(methodCall);
            return parsed;
        }

        protected Expression GetExpressionFromExpressionMethod(MethodCallExpression methodExpression)
        {
            List<object> args = new List<object> { null };
            for (int i = 1; i < methodExpression.Arguments.Count; i++)
            {
                args.Add(methodExpression.Arguments[i].CachedCompileInvoke());
            }
            var returnValue = methodExpression.Method.Invoke(null, args.ToArray());
            var expression =
                typeof(DeligateRecursiveBuilder).MakeGenericType(
                    methodExpression.Method.ReturnType.GetGenericArguments()[0]).GetProperty("Expression").
                    GetGetMethod().Invoke(returnValue, new object[0]);
            var expressionBody = (Expression)
                typeof(Expression<>).GetProperty("Body").GetGetMethod().Invoke(expression, new object[0]);
            var invocationTarget = ((LambdaExpression)expression).Parameters[0];
            var actualInvocationTarget = methodExpression.Arguments[0];

            return expressionBody.Replace<Expression>(x => x == invocationTarget, x => actualInvocationTarget);
        }

        protected Recursion GetFromDelegateBuilderMethod(MethodCallExpression methodExpression, string fieldName)
        {
            if (fieldName.IsNull())
            {
                fieldName = string.Empty;
            }

            List<object> args = new List<object> { null };
            for (int i = 1; i < methodExpression.Arguments.Count; i++)
            {
                args.Add(methodExpression.Arguments[i].CachedCompileInvoke());
            }

            var returnValue = (DeligateRecursiveBuilder)methodExpression.Method.Invoke(null, args.ToArray());

            if (IsFieldNameExpression(methodExpression.Arguments[0]))
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    fieldName = fieldName + ".";
                }

                fieldName = fieldName + methodExpression.Arguments[0].GetFieldPath();
            }

            return returnValue.GetReturnType(fieldName);
        }

        private static bool IsFieldNameExpression(Expression expression)
        {
            if (expression is MemberExpression || expression is MethodCallExpression)
            {
                return true;
            }

            var returnType = expression.GetReturnType();
            if (typeof(Enum).IsAssignableFrom(returnType))
            {
                return true;
            }

            if (returnType.IsGenericType)
            {
                var genericType = returnType.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    var valueType = returnType.GetGenericArguments()[0];
                    if (typeof(Enum).IsAssignableFrom(valueType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
