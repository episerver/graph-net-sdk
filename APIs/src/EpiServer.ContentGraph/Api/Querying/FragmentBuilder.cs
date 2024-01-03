﻿using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL.Transport;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class FragmentBuilder<T> : FragmentBuilder
    {
        public FragmentBuilder() : base() { }
        public FragmentBuilder(string name)
        {
            _query.OperationName = name;
        }
        private FragmentBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            base.Field(fieldSelector.GetFieldPath());
            return this;
        }
        public FragmentBuilder<T> Fields(params Expression<Func<T, object>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var fieldSelector in fieldSelectors)
            {
                Field(fieldSelector);
            }
            return this;
        }
        public FragmentBuilder<T> Link<TLink>(TypeQueryBuilder<TLink> link)
        {
            base.Link(link);
            return this;
        }
        public FragmentBuilder<T> Children<TChildren>(TypeQueryBuilder<TChildren> children)
        {
            base.Children(children);
            return this;
        }
        public override GraphQLRequest GetQuery()
        {
            _query.Query = $"fragment {_query.OperationName} on {typeof(T).Name} {graphObject}";
            return _query;
        }
    }

    public class FragmentBuilder : BaseTypeQueryBuilder
    {
        public FragmentBuilder() : base()
        {
            _query.OperationName = "sampleFragment";
        }
        public void OperationName(string name)
        {
            Regex reg = new Regex(@"^[a-zA-Z_]\w*$");
            if (reg.IsMatch(name))
            {
                _query.OperationName = name;
            }
        }
        public string GetName()
        {
            return _query.OperationName;
        }
    }
}
