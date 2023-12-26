using System;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public static class ExpressionExtensions
    {
        public static Type GetMemberReturnType(this MemberExpression memberExpression)
        {
            return memberExpression.Member.GetMemberReturnType();
        }

        public static string GetFieldPath(this Expression fieldSelector)
        {
            var visitor = new FieldPathVisitor(fieldSelector);
            return visitor.GetPath();
        }

        public static Type GetReturnType(this Expression expression)
        {
            if (expression is LambdaExpression)
            {
                expression = ((LambdaExpression)expression).Body;
            }
            Type fieldType = null;

            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;
                fieldType = memberExpression.Type;
            }
            else if (expression is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression) expression;
                fieldType = methodExpression.Method.ReturnType;
            }
            else if (expression is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression;
                fieldType = unaryExpression.Operand.Type;
            }
            else if (expression is ParameterExpression)
            {
                var parameterExpression = (ParameterExpression)expression;
                fieldType = parameterExpression.Type;
            }

            if (fieldType.IsNull())
            {
                throw new ApplicationException(
                    string.Format(
                        "Unable to retrieve the field type (such as return value) from expression of type {0}.",
                        expression.GetType().Name));
            }
            return fieldType;
        }

        public static Type GetDeclaringType(this Expression fieldSelector)
        {
            Type member = null;

            var selector = fieldSelector as LambdaExpression;
            if (selector.IsNotNull())
            {
                var memberExpression = selector.Body as MemberExpression;
                if (memberExpression.IsNotNull())
                    member = memberExpression.Member.DeclaringType;
                var methodCallExpression = selector.Body as MethodCallExpression;
                if (methodCallExpression.IsNotNull())
                    member = methodCallExpression.Method.DeclaringType;
            }
            if (fieldSelector is MemberExpression)
            {
                member = ((MemberExpression)(fieldSelector)).Member.DeclaringType;
            }

            if (fieldSelector is MethodCallExpression)
            {
                member = ((MethodCallExpression)(fieldSelector)).Method.DeclaringType;
            }
            return member;
        }
    }
}
