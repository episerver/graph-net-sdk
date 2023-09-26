using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EPiServer.Find.ClientConventions
{
    public interface INestedConventionItemWrapper<TSource>
    {
        void Add<TListItem>(Expression<Func<TSource, IEnumerable<TListItem>>> expr) where TListItem : class;
        [Obsolete("IEnumerable<string> cannot be a used to setup a nested convention")]
        void Add(Expression<Func<TSource, IEnumerable<string>>> expr);
    }
}