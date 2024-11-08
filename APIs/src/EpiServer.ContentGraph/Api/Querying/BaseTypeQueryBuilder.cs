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
                AppendItem(clonedPropName);
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
                AppendItem($"{alias}:{clonedPropName}");
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
                AppendItem($"{clonedPropName}{highLightOptions.Query}");
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
                    AppendItem($"_link(type:{linkQueryBuilder.GetLinkType()})");
                }
                AppendItem($"{{{linkQuery}}}");
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
                        AppendItem($"_link(type:{linkQueryBuilder.GetLinkType()})");
                    }
                    else
                    {
                        AppendItem($"{alias}:_link(type:{linkQueryBuilder.GetLinkType()})");
                    }  
                    
                }
                AppendItem($"{{{linkQuery}}}");

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
                AppendItem($"_children{{{childrenItems}}}");
            }

            return this;
        }
        public virtual BaseTypeQueryBuilder AddFragments(params FragmentBuilder[] fragments)
        {
            fragments.ValidateNotNullArgument("fragments");
            foreach (var fragment in fragments)
            {
                AddFragment(fragment);
            }
            return this;
        }
        protected virtual BaseTypeQueryBuilder AddFragment(FragmentBuilder fragment)
        {
            AddFragment(null, fragment);
            return this;
        }
        public virtual BaseTypeQueryBuilder AddFragment(string fieldPath, FragmentBuilder fragment)
        {
            fragment.ValidateNotNullArgument("fragment");
            string propName;
            if (fieldPath.IsNullOrEmpty())
            {
                propName = $"...{fragment.GetName()}";
            }
            else
            {
                var newPath = $"{fieldPath}.$$${fragment.GetName()}";
                //trick: fragment init by $$$ then replace by dots ...
                propName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(newPath);
                propName = propName.Replace("$", ".");
            }

            AppendItem(propName);

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
                    if (Parent.HasFragment(child.GetName()))
                    {
                        continue;
                    }
                    else
                    {
                        yield return child;
                    }
                    
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

        protected virtual void AppendItem(string item)
        {
            if (!item.IsNullOrEmpty())
            {
                graphObject.SelectItems.Append(graphObject.SelectItems.Length > 0 ? $" {item}": item);
            }
        }
    }
}
