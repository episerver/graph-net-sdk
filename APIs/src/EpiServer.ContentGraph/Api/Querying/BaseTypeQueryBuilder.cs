using EPiServer.ContentGraph.Helpers;
using GraphQL.Transport;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class BaseTypeQueryBuilder : ITypeQueryBuilder
    {
        protected readonly ContentGraphQuery graphObject;
        protected readonly GraphQLRequest _query;
        protected bool _compiled = false;
        private IQuery _parent = null;
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
                if (graphObject.SelectItems.IsNullOrEmpty())
                {
                    graphObject.SelectItems = $"{clonedPropName}";
                }
                else
                {
                    graphObject.SelectItems += $" {clonedPropName}";
                }
            }

            return this;
        }
        public virtual BaseTypeQueryBuilder Link(BaseTypeQueryBuilder link)
        {
            link.ValidateNotNullArgument("link");
            string linkItems = link.GetQuery()?.Query ?? string.Empty;
            if (!linkItems.IsNullOrEmpty())
            {
                graphObject.SelectItems += graphObject.SelectItems.IsNullOrEmpty() ?
                    $"_link{{{linkItems}}}" :
                    $" _link{{{linkItems}}}";
            }
            return this;
        }
        public virtual BaseTypeQueryBuilder Children(BaseTypeQueryBuilder children)
        {
            children.ValidateNotNullArgument("children");
            string childrenItems = children.GetQuery()?.Query ?? string.Empty;
            if (!childrenItems.IsNullOrEmpty())
            {
                graphObject.SelectItems += graphObject.SelectItems.IsNullOrEmpty() ?
                    $"_children{{{childrenItems}}}" :
                    $" _children{{{childrenItems}}}";
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
            graphObject.SelectItems += graphObject.SelectItems.IsNullOrEmpty() ? $"...{fragment.GetName()}" : $" ...{fragment.GetName()}";
            Parent?.AddFragment(fragment);
            return this;
        }
    }
}
