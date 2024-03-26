using EPiServer.ContentGraph.Helpers;
using GraphQL.Transport;
using System;

namespace EPiServer.ContentGraph.Api.Querying
{
    public abstract class BaseTypeQueryBuilder : ITypeQueryBuilder
    {
        protected readonly ContentGraphQuery graphObject;
        protected readonly GraphQLRequest _query;
        protected bool _compiled = false;
        protected IQuery _parent = null;
        public virtual IQuery Parent
        {
            get => _parent;
            set
            {
                if (_parent.IsNull())
                {
                    _parent = value;
                }
            }
        }

        public BaseTypeQueryBuilder()
        {
            graphObject = new ContentGraphQuery();
            _query = new GraphQLRequest();
        }
        public BaseTypeQueryBuilder(GraphQLRequest query)
        {
            graphObject = new ContentGraphQuery();
            _query = query;
        }

        public virtual GraphQueryBuilder ToQuery()
        {
            if (!_compiled)
            {
                _compiled = true;
                _query.Query = graphObject.ToString();
            }
            return new GraphQueryBuilder(_query, this);
        }

        public virtual GraphQLRequest GetQuery()
        {
            ToQuery();
            return _query;
        }

        public virtual BaseTypeQueryBuilder Field(string propertyName)
        {
            propertyName.ValidateNotNullArgument("propertyName");
            if (!propertyName.IsNullOrEmpty())
            {
                string clonedPropName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName);
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"{clonedPropName}" :
                    $" {clonedPropName}"
                );
            }

            return this;
        }
        public virtual BaseTypeQueryBuilder Link(BaseTypeQueryBuilder link)
        {
            link.ValidateNotNullArgument("link");
            string linkItems = link.GetQuery()?.Query ?? string.Empty;
            if (!linkItems.IsNullOrEmpty())
            {
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"_link{{{linkItems}}}" :
                    $" _link{{{linkItems}}}"
                );
            }
            return this;
        }
        [Obsolete("Use Link method instead")]
        public virtual BaseTypeQueryBuilder Children(BaseTypeQueryBuilder children)
        {
            children.ValidateNotNullArgument("children");
            string childrenItems = children.GetQuery()?.Query ?? string.Empty;
            if (!childrenItems.IsNullOrEmpty())
            {
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"_children{{{childrenItems}}}" :
                    $" _children{{{childrenItems}}}"
                );
            }

            return this;
        }
        public virtual BaseTypeQueryBuilder Fragments(params FragmentBuilder[] fragments)
        {
            fragments.ValidateNotNullArgument("fragments");
            foreach (var fragment in fragments)
            {
                Fragment(fragment);
            }
            return this;
        }
        protected virtual BaseTypeQueryBuilder Fragment(FragmentBuilder fragment)
        {
            fragment.ValidateNotNullArgument("fragment");
            graphObject.SelectItems.Append(
                graphObject.SelectItems.Length == 0 ? 
                $"...{fragment.GetName()}" : 
                $" ...{fragment.GetName()}"
            );

            if (Parent != null)
            {
                Parent.AddFragment(fragment);
                var children = GetAllChildren(fragment);
                foreach (var childFragment in children)
                {
                    Parent.AddFragment(childFragment);
                }
            }
            return this;
        }
        private IEnumerable<FragmentBuilder> GetAllChildren(FragmentBuilder fragment)
        {
            if (fragment.HasChildren)
            {
                foreach (var child in fragment.ChildrenFragments)
                {
                    yield return child;
                    if (child.HasChildren)
                    {
                        foreach (var item in GetAllChildren(child))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }
    }
}
