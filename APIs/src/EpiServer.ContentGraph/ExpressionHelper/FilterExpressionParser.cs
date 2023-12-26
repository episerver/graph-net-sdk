using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Linq;
using EPiServer.ContentGraph.Helpers.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    public class FilterExpressionParser
    {
        public virtual Filter GetFilter<TSource>(Expression<Func<TSource, Filter>> filterExpression)
        {
            ValidateFilterExpression(filterExpression);

            Expression executable = filterExpression.Body;

            //Find and replace methods returning FilterExpression
            while (executable.Find<MethodCallExpression>(ReturnsFilterExpression).Count() > 0)
            {
                executable = executable.Replace<MethodCallExpression>(
                    ReturnsFilterExpression,
                    RealizeFilterExpressionMethodCalls);
            }
            //Find and replace methods returning DelegateFilterBuilder
            executable = executable.Replace<MethodCallExpression>(
                x => x.Method.ReturnType == typeof(DelegateFilterBuilder),
                x => Expression.Constant(GetFilterFromDelegateFilterBuilderMethod(x, null)));

            return executable.CachedCompileInvoke<Filter>();
        }

        protected virtual void ValidateFilterExpression<TSource>(Expression<Func<TSource, Filter>> filterExpression)
        {
            if (filterExpression.Body.Find<NewExpression>(x => x.Type == typeof(DelegateFilterBuilder)).Count() > 0)
            {
                throw new NotSupportedException
                    (string.Format("Instantiating new {0} is not supported."
                                   + "The {0} class is intended to be used as a return value from extensions methods.",
                                   typeof(DelegateFilterBuilder).Name));
            }
            var methodsReturningFilterBuilder =
                filterExpression.Body.Find<MethodCallExpression>(
                    x => !x.Method.IsExtensionMethod() && x.Method.ReturnType == typeof(DelegateFilterBuilder));
            if (methodsReturningFilterBuilder.Count() > 0)
            {
                throw new NotSupportedException(
                    string.Format(
                        "Method {0} returns {1}. Only extension methods should return {1}.",
                        methodsReturningFilterBuilder.First().Method.Name,
                        typeof(DelegateFilterBuilder).Name));
            }
            if (
                filterExpression.Body.Find<NewExpression>(
                    x => x.Type.IsGenericType && x.Type.GetGenericTypeDefinition() == typeof(FilterExpression<>)).Count() > 0)
            {
                throw new NotSupportedException
                    (string.Format("Instantiating new {0} is not supported."
                                   + "The {0} class is intended to be used as a return value from extensions methods.",
                                   typeof(FilterExpression<>).Name));
            }
            var methodsRetuningFilterExpression =
                filterExpression.Body.Find<MethodCallExpression>(
                    x =>
                    !x.Method.IsExtensionMethod() && x.Type.IsGenericType &&
                    x.Type.GetGenericTypeDefinition() == typeof(FilterExpression<>));
            if (methodsRetuningFilterExpression.Count() > 0)
            {
                throw new NotSupportedException(
                    string.Format(
                        "Method {0} returns {1}. Only extension methods should return {1}.",
                        methodsRetuningFilterExpression.First().Method.Name,
                        typeof(FilterExpression<>).Name));
            }
        }

        protected bool ReturnsFilterExpression(MethodCallExpression x)
        {
            return x.Method.HasGenericTypeDefinition(typeof(FilterExpression<>));
        }

        protected Expression RealizeFilterExpressionMethodCalls(MethodCallExpression methodCall)
        {
            var parsed = GetExpressionFromFilterExpressionMethod(methodCall);
            return parsed;
        }

        protected Expression GetExpressionFromFilterExpressionMethod(MethodCallExpression methodExpression)
        {
            List<object> args = new List<object> { null };
            for (int i = 1; i < methodExpression.Arguments.Count; i++)
            {
                args.Add(methodExpression.Arguments[i].CachedCompileInvoke());
            }
            var returnValue = methodExpression.Method.Invoke(null, args.ToArray());
            var expression =
                typeof(FilterExpression<>).MakeGenericType(
                    methodExpression.Method.ReturnType.GetGenericArguments()[0]).GetProperty("Expression").
                    GetGetMethod().Invoke(returnValue, new object[0]);
            var expressionBody = (Expression)
                typeof(Expression<>).GetProperty("Body").GetGetMethod().Invoke(expression, new object[0]);
            var invocationTarget = ((LambdaExpression)expression).Parameters[0];
            var actualInvocationTarget = methodExpression.Arguments[0];

            return expressionBody.Replace<Expression>(x => x == invocationTarget, x => actualInvocationTarget);
        }

        protected Filter GetFilterFromDelegateFilterBuilderMethod(MethodCallExpression methodExpression, string fieldName)
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

            var returnValue = (DelegateFilterBuilder)methodExpression.Method.Invoke(null, args.ToArray());

            if (IsFieldNameExpression(methodExpression.Arguments[0]))
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    fieldName = fieldName + ".";
                }

                fieldName = fieldName + methodExpression.Arguments[0].GetFieldPath();
            }

            return returnValue.GetFilter(fieldName);
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
