﻿using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Linq;
using EPiServer.ContentGraph.Helpers.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.ExpressionHelper
{
    public class FacetExpressionParser
    {
        public virtual FacetFilter GetFacetFilter<TSource>(Expression<Func<TSource, FacetFilter>> facetFilterExpression)
        {
            ValidateFacetFilterExpression(facetFilterExpression);

            Expression executable = facetFilterExpression.Body;

            //Find and replace methods returning FilterExpression
            var methods = executable.Find<MethodCallExpression>(ReturnsFacetFilterExpression);
            while (executable.Find<MethodCallExpression>(ReturnsFacetFilterExpression).Count() > 0)
            {
                executable = executable.Replace<MethodCallExpression>(
                    ReturnsFacetFilterExpression,
                    RealizeFacetFilterExpressionMethodCalls);
            }
            //Find and replace methods returning DelegateFacetFilterBuilder
            executable = executable.Replace<MethodCallExpression>(
                x => x.Method.ReturnType == typeof(DelegateFacetFilterBuilder),
                x => Expression.Constant(GetFilterFromDelegateFacetFilterBuilderMethod(x, null)));

            return executable.CachedCompileInvoke<FacetFilter>();
        }

        protected virtual void ValidateFacetFilterExpression<TSource>(Expression<Func<TSource, FacetFilter>> facetFilterExpression)
        {
            if (facetFilterExpression.Body.Find<NewExpression>(x => x.Type == typeof(DelegateFacetFilterBuilder)).Count() > 0)
            {
                throw new NotSupportedException
                    (string.Format("Instantiating new {0} is not supported."
                                   + "The {0} class is intended to be used as a return value from extensions methods.",
                                   typeof(DelegateFacetFilterBuilder).Name));
            }
            var methodsReturningFilterBuilder =
                facetFilterExpression.Body.Find<MethodCallExpression>(
                    x => !x.Method.IsExtensionMethod() && x.Method.ReturnType == typeof(DelegateFacetFilterBuilder));
            if (methodsReturningFilterBuilder.Count() > 0)
            {
                throw new NotSupportedException(
                    string.Format(
                        "Method {0} returns {1}. Only extension methods should return {1}.",
                        methodsReturningFilterBuilder.First().Method.Name,
                        typeof(DelegateFacetFilterBuilder).Name));
            }
            if (facetFilterExpression.Body.Find<NewExpression>(
                    x => x.Type.IsGenericType && x.Type.GetGenericTypeDefinition() == typeof(FacetExpression<>)).Count() > 0)
            {
                throw new NotSupportedException($"Instantiating new {typeof(FacetExpression<>).Name} is not supported. " +
                    $"The {typeof(FacetExpression<>).Name} class is intended to be used as a return value from extensions methods.");
            }
            var methodsRetuningFilterExpression =
                facetFilterExpression.Body.Find<MethodCallExpression>(
                    x =>
                    !x.Method.IsExtensionMethod() && x.Type.IsGenericType &&
                    x.Type.GetGenericTypeDefinition() == typeof(FacetExpression<>));
            if (methodsRetuningFilterExpression.Count() > 0)
            {
                throw new NotSupportedException($"Method {methodsRetuningFilterExpression.First().Method.Name} returns {typeof(FacetExpression<>).Name}. " +
                    $"Only extension methods should return {typeof(FacetExpression<>).Name}.");
            }
        }

        protected Expression GetExpressionFromFacetFilterExpressionMethod(MethodCallExpression methodExpression)
        {
            List<object> args = new List<object> { null };
            for (int i = 1; i < methodExpression.Arguments.Count; i++)
            {
                args.Add(methodExpression.Arguments[i].CachedCompileInvoke());
            }
            var returnValue = methodExpression.Method.Invoke(null, args.ToArray());
            var expression =
                typeof(FacetExpression<>).MakeGenericType(
                    methodExpression.Method.ReturnType.GetGenericArguments()[0]).GetProperty("Expression").
                    GetGetMethod().Invoke(returnValue, new object[0]);
            var expressionBody = (Expression)
                typeof(Expression<>).GetProperty("Body").GetGetMethod().Invoke(expression, new object[0]);
            var invocationTarget = ((LambdaExpression)expression).Parameters[0];
            var actualInvocationTarget = methodExpression.Arguments[0];

            return expressionBody.Replace<Expression>(x => x == invocationTarget, x => actualInvocationTarget);
        }

        protected FacetFilter GetFilterFromDelegateFacetFilterBuilderMethod(MethodCallExpression methodExpression, string fieldName)
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

            var returnValue = (DelegateFacetFilterBuilder)methodExpression.Method.Invoke(null, args.ToArray());

            if (IsFieldNameExpression(methodExpression.Arguments[0]))
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    fieldName = fieldName + ".";
                }

                fieldName = fieldName + methodExpression.Arguments[0].GetFieldPath();
            }

            return returnValue.GetFacetFilter(fieldName);
        }
        protected bool ReturnsFacetFilterExpression(MethodCallExpression x)
        {
            return x.Method.HasGenericTypeDefinition(typeof(FacetExpression<>));
        }

        protected Expression RealizeFacetFilterExpressionMethodCalls(MethodCallExpression methodCall)
        {
            var parsed = GetExpressionFromFacetFilterExpressionMethod(methodCall);
            return parsed;
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
