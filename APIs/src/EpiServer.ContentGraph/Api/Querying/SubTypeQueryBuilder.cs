using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;
using System.Text;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class SubTypeQueryBuilder<T>
    {
        private StringBuilder builder;
        public SubTypeQueryBuilder()
        {
            builder = new StringBuilder();
        }
        public string Query { get { return builder.ToString(); } }
        public SubTypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var propertyName = fieldSelector.GetFieldPath();
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }
            builder.Append(ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName));
            return this;
        }
    }
}
