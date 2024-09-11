using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL.Transport;
using System;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class SubTypeQueryBuilder<T> : BaseTypeQueryBuilder
    {
        public SubTypeQueryBuilder():base()
        {
        }
        public override GraphQLRequest GetQuery()
        {
            return base.GetQuery();
        }
        public override SubTypeQueryBuilder<T> Field(string fieldName)
        {
            fieldName.ValidateNotNullArgument("fieldName");
            base.Field(fieldName);
            return this;
        }
        public SubTypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            base.Field(fieldSelector.GetFieldPath());
            return this;
        }
        public SubTypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector, string alias)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var propertyName = fieldSelector.GetFieldPath();
            base.Field(propertyName, alias);
            return this;
        }
        public SubTypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector, HighLightOptions highLightOptions, string alias = "")
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var propertyName = fieldSelector.GetFieldPath();
            if (string.IsNullOrEmpty(alias))
            {
                base.Field($"{propertyName}", highLightOptions);
            }
            else
            {
                base.Field($"{alias}:{propertyName}", highLightOptions);
            }
            return this;
        }
        public SubTypeQueryBuilder<T> Fields(params Expression<Func<T, object>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var fieldSelector in fieldSelectors)
            {
                Field(fieldSelector);
            }
            return this;
        }
        public SubTypeQueryBuilder<T> Link<TLink>(TypeQueryBuilder<TLink> link)
        {
            base.Link(link);
            return this;
        }
        [Obsolete("Use Link method instead")]
        public SubTypeQueryBuilder<T> Children<TChildren>(TypeQueryBuilder<TChildren> children)
        {
            base.Children(children);
            return this;
        }
        public override SubTypeQueryBuilder<T> AddFragments(params FragmentBuilder[] fragments)
        {
            base.AddFragments(fragments);
            return this;
        }
        protected override SubTypeQueryBuilder<T> AddFragment(FragmentBuilder fragment)
        {
            base.AddFragment(fragment);
            return this;
        }
    }
}
