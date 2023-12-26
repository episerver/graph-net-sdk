using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System;
using System.Linq.Expressions;
using System.Text;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class SubTypeQueryBuilder<T> : BaseTypeQueryBuilder
    {
        private StringBuilder builder;
        public SubTypeQueryBuilder()
        {
            builder = new StringBuilder();
        }
        public string Query { get { return builder.ToString(); } }

        public override GraphQueryBuilder ToQuery()
        {
            _query.Query = builder.ToString();
            return new GraphQueryBuilder(_query, this);
        }

        public SubTypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var propertyName = fieldSelector.GetFieldPath();
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }
            builder.Append(ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName));
            return this;
        }
        public SubTypeQueryBuilder<T> Fields(params Expression<Func<T, object>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var fieldSelector in fieldSelectors)
            {
                var propertyName = fieldSelector.GetFieldPath();
                if (builder.Length > 0)
                {
                    builder.Append(' ');
                }
                builder.Append(ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName));
            }
            return this;
        }
    }
}
