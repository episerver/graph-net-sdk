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
        public SubTypeQueryBuilder<T> Children<TChildren>(TypeQueryBuilder<TChildren> children)
        {
            base.Children(children);
            return this;
        }
        public override SubTypeQueryBuilder<T> Fragments(params FragmentBuilder[] fragments)
        {
            base.Fragments(fragments);
            return this;
        }
        protected override SubTypeQueryBuilder<T> Fragment(FragmentBuilder fragment)
        {
            base.Fragment(fragment);
            return this;
        }
    }
}
