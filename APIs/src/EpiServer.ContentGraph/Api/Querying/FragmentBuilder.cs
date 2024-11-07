using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Extensions;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL.Transport;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class Recursion
    {
        public string FieldName { get; set; }
        public int? RecursiveDepth { get; set; }
        public override string ToString()
        {
            if (RecursiveDepth.IsNull() || RecursiveDepth.Value < 0)
            {
                return ConvertNestedFieldToString.ConvertNestedFieldForQuery($"{FieldName} @recursive");
            }
            else
            {
                return ConvertNestedFieldToString.ConvertNestedFieldForQuery($"{FieldName} @recursive(depth:{RecursiveDepth.Value})");
            }
        }
    }
    public class FragmentBuilder : BaseTypeQueryBuilder
    {
        private List<FragmentBuilder> _childrenFragments;
        public IEnumerable<FragmentBuilder> ChildrenFragments => _childrenFragments;
        public bool HasChildren => _childrenFragments != null && _childrenFragments.Any();
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
        public override FragmentBuilder AddFragments(params FragmentBuilder[] fragments)
        {
            if (fragments.IsNotNull() && fragments.Length > 0)
            {
                if (_childrenFragments.IsNull())
                {
                    _childrenFragments = new List<FragmentBuilder>();
                }
                base.AddFragments(fragments);
                _childrenFragments.AddRange(fragments);
            }
            return this;
        }
        public FragmentBuilder AddFragment(string path, FragmentBuilder fragment)
        {
            if (_childrenFragments.IsNull())
            {
                _childrenFragments = new List<FragmentBuilder>();
            }
            base.AddFragment(path, fragment);
            _childrenFragments.Add(fragment);

            return this;
        }
    }
    
    public class FragmentBuilder<T> : FragmentBuilder
    {
        public FragmentBuilder() : base() { }
        public FragmentBuilder(string name)
        {
            base.OperationName(name);
        }
        private FragmentBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            base.Field(fieldSelector.GetFieldPath());
            return this;
        }
        public FragmentBuilder<T> Field(Expression<Func<T, object>> fieldSelector, string alias)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            base.Field(fieldSelector.GetFieldPath(), alias);
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
        [Obsolete("Obsoleted. Use Link instead")]
        public FragmentBuilder<T> Children<TChildren>(TypeQueryBuilder<TChildren> children)
        {
            base.Children(children);
            return this;
        }
        private FragmentBuilder Recursive<TSub>(params Recursion[] recursives) where TSub : T
        {
            recursives.ValidateNotNullArgument("recursives");
            if (!recursives.Any()) throw new ArgumentException("recursives can not be empty");
            string recursiveNess = string.Empty;
            foreach (var recursive in recursives)
            {
                recursiveNess += recursive.ToString();
            }

            string subTypeName = typeof(TSub).Name;
            AppendItem($"... on {subTypeName}{{{recursiveNess}}}");
            return this;
        }
        private FragmentBuilder<T> Recursive<TSub>(string fieldName, int? depth = null) where TSub : T
        {
            Recursive<TSub>(new Recursion() { FieldName = fieldName, RecursiveDepth = depth }); ;
            return this;
        }
        /// <summary>
        /// Recursive directive for a subtype
        /// </summary>
        /// <typeparam name="TSub">Subtype must inherits from type <typeparamref name="T"/></typeparam>
        /// <param name="fieldSelectors">Select field must inherits from <typeparamref name="T"/></param>
        /// <returns>FragmentBuilder</returns>
        public FragmentBuilder<T> Recursive<TSub>(Expression<Func<TSub, T>> fieldSelector, int? depth = null) where TSub : T
        {
            var fieldName = fieldSelector.GetFieldPath();
            Recursive<TSub>(fieldName, depth);
            return this;
        }
        /// <summary>
        /// Inline fragment on a subtype
        /// </summary>
        /// <typeparam name="TSub"></typeparam>
        /// <param name="fieldSelectors"></param>
        /// <returns></returns>
        public FragmentBuilder<T> Inline<TSub>(params string[] fields)
        {
            fields.ValidateNotNullArgument("fields");
            StringBuilder propertyBuilder = new StringBuilder();
            foreach (var field in fields)
            {
                if (propertyBuilder.Length > 0)
                {
                    propertyBuilder.Append(" ");
                }
                propertyBuilder.Append(ConvertNestedFieldToString.ConvertNestedFieldForQuery(field));
            }
            string subTypeName = typeof(TSub).Name;
            AppendItem($"... on {subTypeName}{{{propertyBuilder}}}");
            return this;
        }
        /// <summary>
        /// Inline fragment on a subtype
        /// </summary>
        /// <typeparam name="TSub"></typeparam>
        /// <param name="fieldSelectors"></param>
        /// <returns></returns>
        public FragmentBuilder<T> Inline<TSub>(params Expression<Func<TSub, object>>[] fieldSelectors) where TSub : T
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            StringBuilder propertyBuilder = new StringBuilder();
            foreach (var fieldSelector in fieldSelectors)
            {
                if (propertyBuilder.Length > 0)
                {
                    propertyBuilder.Append(" ");
                }
                propertyBuilder.Append(ConvertNestedFieldToString.ConvertNestedFieldForQuery(fieldSelector.GetFieldPath()));
            }
            string subTypeName = typeof(TSub).Name;
            AppendItem($"... on {subTypeName}{{{propertyBuilder}}}");
            return this;
        }
        /// <summary>
        /// Inline fragment on a subtype
        /// </summary>
        /// <typeparam name="TSub"></typeparam>
        /// <param name="fieldSelectors"></param>
        /// <returns></returns>
        public FragmentBuilder<T> Inline<TSub>(params Expression<Func<TSub, Recursion>>[] fieldSelectors) where TSub : T
        {
            fieldSelectors.ValidateNotNullOrEmptyArgument("fieldSelectors");
            var paser = new FragmentExpressionParser();
            Recursive<TSub>(fieldSelectors.Select(selector => paser.GetReturnType(selector)).ToArray());
            return this;
        }
        public FragmentBuilder<T> AddFragment<TProp>(Expression<Func<T, TProp>> fieldSelector, FragmentBuilder<TProp> fragment)
        {
            fieldSelector.ValidateNotNullArgument(nameof(fieldSelector));
            fragment.ValidateNotNullArgument(nameof(fragment));

            var fieldPath = fieldSelector.GetFieldPath();
            base.AddFragment(fieldPath, fragment);
            return this;
        }
        public override GraphQLRequest GetQuery()
        {
            _query.Query = $"fragment {_query.OperationName} on {typeof(T).Name} {graphObject}";
            return _query;
        }
    }
}
