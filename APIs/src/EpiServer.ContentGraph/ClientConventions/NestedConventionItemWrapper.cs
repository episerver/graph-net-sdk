using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EPiServer.Find.ClientConventions
{
    public class NestedConventionItemInstanceWrapper<TSource> : INestedConventionItemWrapper<TSource> where TSource : class
    {
        private readonly NestedConventions _nestedConventions;

        public NestedConventionItemInstanceWrapper(NestedConventions nestedConventions)
        {
            _nestedConventions = nestedConventions;
        }

        public void Add<TListItem>(Expression<Func<TSource, IEnumerable<TListItem>>> expr) where TListItem : class
        {
            _nestedConventions.ForInstancesOf(expr);
        }

        public void Add(Expression<Func<TSource, IEnumerable<string>>> expr)
        {
            throw new ArgumentException("IEnumerable<string> cannot be a used to setup a nested convention");
        }
    }

    public class NestedConventionItemTypeWrapper<TSource> : INestedConventionItemWrapper<TSource> where TSource : class
    {
        private readonly NestedConventions _nestedConventions;

        public NestedConventionItemTypeWrapper(NestedConventions nestedConventions)
        {
            _nestedConventions = nestedConventions;
        }

        public void Add<TListItem>(Expression<Func<TSource, IEnumerable<TListItem>>> expr) where TListItem : class
        {
            _nestedConventions.ForType(expr);
        }

        public void Add(Expression<Func<TSource, IEnumerable<string>>> expr)
        {
            throw new ArgumentException("IEnumerable<string> cannot be a used to setup a nested convention");
        }
    }
}