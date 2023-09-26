using EPiServer.ContentGraph.Helpers.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Helpers
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static bool IsNotNull(this object value)
        {
            return !value.IsNull();
        }

        public static bool IsNot<T>(this object instance)
        {
            return !(instance is T);
        }

        internal static bool IsNullOrEmpty(this object? value)
        {
            return value.IsNull() || (value is string str && string.IsNullOrWhiteSpace(str));
        }

        public static IEnumerable<Type> GetObjectTypesAndInterfaces(this object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var types = instance.GetObjectTypes();
            return types.Concat(instance.GetType().GetInterfaces());
        }

        public static IEnumerable<Type> GetObjectTypes(this object instance)
        {
            instance.ValidateNotNullArgument("instance");
            return instance.GetType().GetTypes();
        }

        public static void ValidateNotNullArgument(this object argument, string paramName)
        {
            if (argument.IsNull())
            {
                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod();
                var message =
                    string.Format(
                        "The method {0} in class {1} has been invoked with null as value for the argument {2}.",
                        method.Name,
                        method.DeclaringType,
                        paramName);
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static void ValidateTypeArgument<T>(this object argument, string paramName)
        {
            if (!(argument is T))
            {
                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod();
                var message =
                    string.Format(
                        "The method {0} in class {1} has been invoked with an invalid type {2} as value for the argument {3}.",
                        method.Name,
                        method.DeclaringType,
                        argument.GetType(),
                        paramName);
                throw new ArgumentException(paramName, message);
            }
        }

        // source: https://stackoverflow.com/a/2081942
        public static TResult IfNotNull<TArg, TResult>(this TArg arg, Expression<Func<TArg, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (ReferenceEquals(arg, null))
            {
                return default(TResult);
            }

            var stack = new Stack<MemberExpression>();
            var expr = expression.Body as MemberExpression;
            while (expr != null)
            {
                stack.Push(expr);
                expr = expr.Expression as MemberExpression;
            }

            if (stack.Count == 0 || !(stack.Peek().Expression is ParameterExpression))
            {
                throw new ApplicationException(String.Format("The expression '{0}' contains unsupported constructs.", expression));
            }

            object a = arg;
            while (stack.Count > 0)
            {
                expr = stack.Pop();
                var p = expr.Expression as ParameterExpression;
                if (p == null)
                {
                    p = Expression.Parameter(a.GetType(), "x");
                    expr = expr.Update(p);
                }
                var lambda = Expression.Lambda(expr, p);
                Delegate t = lambda.Compile();
                a = t.DynamicInvoke(a);
                if (ReferenceEquals(a, null))
                {
                    return default(TResult);
                }
            }

            return (TResult)a;
        }

        internal static void ValidateNotNullOrEmptyArgument(this object argument, string paramName)
        {
            if (argument.IsNullOrEmpty())
            {
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null or empty.");
            }
        }
    }

}
