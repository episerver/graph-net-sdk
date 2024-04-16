using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Text;
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
        public virtual BaseTypeQueryBuilder Field(string propertyName, string alias)
        {
            propertyName.ValidateNotNullArgument("propertyName");
            alias.ValidateNotNullArgument("alias");
            if (!propertyName.IsNullOrEmpty() && !alias.IsNullOrEmpty())
            {
                string clonedPropName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName);
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"{alias}:{clonedPropName}" :
                    $" {alias}:{clonedPropName}"
                );
            }
            else
            {
                Field(propertyName);
            }

            return this;
        }
        public virtual BaseTypeQueryBuilder Field(string propertyName, HighLightOptions highLightOptions)
        {
            propertyName.ValidateNotNullArgument("propertyName");

            if (!propertyName.IsNullOrEmpty())
            {
                if (highLightOptions.IsNull())
                {
                    Field(propertyName);
                    return this;
                }
                string clonedPropName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName);
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"{clonedPropName}{highLightOptions.Query}" :
                    $" {clonedPropName}{highLightOptions.Query}"
                );
            }

            return this;
        }
        public virtual BaseTypeQueryBuilder Link(ITypeQueryBuilder link)
        {
            link.ValidateNotNullArgument("link");
            var linkQueryBuilder = link as ILinkQueryBuilder;
            if (linkQueryBuilder is null)
            {
                throw new ArgumentException("The argument [link] is not type of [LinkQueryBuilder]");
            }
            string linkQuery = linkQueryBuilder.GetQuery()?.Query ?? string.Empty;
            if (!linkQuery.IsNullOrEmpty())
            {
                if (!linkQueryBuilder.GetLinkType().IsNullOrEmpty())
                {
                    graphObject.SelectItems.Append(
                        graphObject.SelectItems.Length == 0 ?
                        $"_link(type:{linkQueryBuilder.GetLinkType()})" :
                        $" _link(type:{linkQueryBuilder.GetLinkType()})"
                    );
                }
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"{{{linkQuery}}}" :
                    $" {{{linkQuery}}}"
                );
            }
            return this;
        }
        public virtual BaseTypeQueryBuilder Link(ITypeQueryBuilder link, string alias)
        {
            link.ValidateNotNullArgument("link");
            var linkQueryBuilder = link as ILinkQueryBuilder;
            if (linkQueryBuilder is null)
            {
                throw new ArgumentException("The argument [link] is not type of [LinkQueryBuilder]");
            }
            if (!alias.IsValidName(50))
            {
                throw new ArgumentException($"Alias name {alias} is not valid");
            }
            string linkQuery = linkQueryBuilder.GetQuery()?.Query ?? string.Empty;
            if (!linkQuery.IsNullOrEmpty())
            {
                if (!linkQueryBuilder.GetLinkType().IsNullOrEmpty())
                {
                    if (string.IsNullOrEmpty(alias))
                    {
                        graphObject.SelectItems.Append(
                            graphObject.SelectItems.Length == 0 ?
                            $"_link(type:{linkQueryBuilder.GetLinkType()})" :
                            $" _link(type:{linkQueryBuilder.GetLinkType()})"
                        );
                    }
                    else
                    {
                        graphObject.SelectItems.Append(
                            graphObject.SelectItems.Length == 0 ?
                            $"{alias}:_link(type:{linkQueryBuilder.GetLinkType()})" :
                            $" {alias}:_link(type:{linkQueryBuilder.GetLinkType()})"
                        );
                    }  
                    
                }
                graphObject.SelectItems.Append(
                    graphObject.SelectItems.Length == 0 ?
                    $"{{{linkQuery}}}" :
                    $" {{{linkQuery}}}"
                );

            }
            return this;
        }
        [Obsolete("Use Link method instead")]
        public virtual BaseTypeQueryBuilder Children(ITypeQueryBuilder children)
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
