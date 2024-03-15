using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL.Transport;
using System.Linq.Expressions;
using System.Globalization;
using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ServiceLocation;
using EPiServer.ContentGraph.ExpressionHelper;
using System.Text.RegularExpressions;

namespace EPiServer.ContentGraph.Api.Querying
{
    public partial class TypeQueryBuilder<T> : TypeQueryBuilder
    {
        private static IEnumerable<IFilterForVisitor>? _filters;
        public TypeQueryBuilder(GraphQLRequest request) : base(request)
        {
        }
        public TypeQueryBuilder() : base()
        {
        }
        public override GraphQLRequest GetQuery()
        {
            ToQuery();
            return _query;
        }

        #region Queries
        public TypeQueryBuilder<T> Link<TLink>(TypeQueryBuilder<TLink> link)
        {
            base.Link(link);
            return this;
        }
        [Obsolete("Use Link method instead")]
        public TypeQueryBuilder<T> Children<TChildren>(TypeQueryBuilder<TChildren> children)
        {
            base.Children(children);
            return this;
        }
        public override TypeQueryBuilder<T> Fragments(params FragmentBuilder[] fragments)
        {
            base.Fragments(fragments);
            return this;
        }
        protected override TypeQueryBuilder<T> Fragment(FragmentBuilder fragment)
        {
            base.Fragment(fragment);
            return this;
        }
        public override TypeQueryBuilder<T> Field(string propertyName)
        {
            base.Field(propertyName);
            return this;
        }
        public TypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var propertyName = fieldSelector.GetFieldPath();
            Field(propertyName);
            return this;
        }
        public TypeQueryBuilder<T> Fields(params Expression<Func<T, object>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var fieldSelector in fieldSelectors)
            {
                Field(fieldSelector);
            }
            return this;
        }
        /// <summary>
        /// Select fields in a subtype. The response of your query may need to convert data type. If then consider to use GetContent<TOriginal,TExpectedType> method.
        /// </summary>
        /// <typeparam name="TSub">Subtype must be inherited from type <typeparamref name="T"/></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> AsType<TSub>(string propertyName) where TSub : T
        {
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"... on {subTypeName}{{{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}}}" :
                $"{graphObject.SelectItems} ... on {subTypeName}{{{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}}}";
            return this;
        }
        /// <summary>
        /// Select fields in a subtype. The response of your query may need to convert data type. If then consider to use GetContent<TOriginal,TExpectedType> method.
        /// </summary>
        /// <typeparam name="TSub">Subtype must be inherited from type <typeparamref name="T"/></typeparam>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> AsType<TSub>(params Expression<Func<TSub, object>>[] fieldSelectors) where TSub : T
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            string propertyName = string.Empty;
            foreach (var fieldSelector in fieldSelectors)
            {
                if (!propertyName.IsNullOrEmpty())
                {
                    propertyName += " ";
                }
                propertyName += ConvertNestedFieldToString.ConvertNestedFieldForQuery(fieldSelector.GetFieldPath());

            }
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"... on {subTypeName}{{{propertyName}}}" :
                $"{graphObject.SelectItems} ... on {subTypeName}{{{propertyName}}}";
            return this;
        }
        /// <summary>
        /// Select fields in a subtype. The response of your query may need to convert data type. If then consider to use GetContent<TOriginal,TExpectedType> method.
        /// </summary>
        /// <typeparam name="TSub">Subtype must be inherited from type <typeparamref name="T"/></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> AsType<TSub>(SubTypeQueryBuilder<TSub> subTypeQuery) where TSub : T
        {
            subTypeQuery.ValidateNotNullArgument("subTypeQuery");
            subTypeQuery.Parent = this.Parent;
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"... on {subTypeName}{subTypeQuery.GetQuery().Query}" :
                $"{graphObject.SelectItems} ... on {subTypeName}{subTypeQuery.GetQuery().Query}";
            return this;
        }

        public TypeQueryBuilder<T> Autocomplete(Expression<Func<T, object>> fieldSelector, AutoCompleteOperators autocomplete)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            autocomplete.ValidateNotNullArgument("autocompleteOperator");
            var propertyName = ConvertNestedFieldToString.ConvertNestedFieldForAutoComplete(fieldSelector.GetFieldPath(), autocomplete);
            if (graphObject.Autocomplete.IsNullOrEmpty())
            {
                graphObject.Autocomplete = $"{propertyName}";
            }
            else
            {
                graphObject.Autocomplete += $" {propertyName}";
            }
            return this;
        }
        /// <summary>
        /// Get facet by field
        /// </summary>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> Facet(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Facet(fieldSelector.GetFieldPath());
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, DateTime>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Facet(fieldSelector.GetFieldPath());
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, bool>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Facet(fieldSelector.GetFieldPath());
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, int>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            Facet(fieldSelector.GetFieldPath());
            return this;
        }
        public TypeQueryBuilder<T> Facet(string propertyName)
        {
            string facet = ConvertNestedFieldToString.ConvertNestedFieldForFacet(propertyName);
            graphObject.Facets = graphObject.Facets.IsNullOrEmpty() ?
                $"{facet}" :
                $"{graphObject.Facets} {facet}";
            return this;
        }
        public TypeQueryBuilder<T> Total(bool? isAll = null)
        {
            if (!isAll.HasValue)
            {
                graphObject.Total = "total";
            }
            else
            {
                graphObject.Total = $"total(all:{isAll.Value.ToString().ToLower()})";
            }

            return this;
        }
        /// <summary>
        /// Get scroll id from search
        /// </summary>
        /// <returns></returns>
        public TypeQueryBuilder<T> GetCursor()
        {
            graphObject.Cursor = "cursor";
            return this;
        }
        #endregion

        #region Filters
        public TypeQueryBuilder<T> Skip(long skip)
        {
            graphObject.Skip = $"skip:{skip}";
            return this;
        }
        /// <summary>
        /// Set scroll id for search query
        /// </summary>
        /// <param name="scrollId">scroll id</param>
        /// <returns></returns>
        public TypeQueryBuilder<T> SetCursor(string scrollId)
        {
            if (!scrollId.IsNullOrEmpty())
            {
                graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ?
                    $"cursor:\"{scrollId}\"" :
                    $"{graphObject.Filter},cursor:\"{scrollId}\"";
            }
            return this;
        }
        public TypeQueryBuilder<T> Ids(params string[] ids)
        {
            ids.ValidateNotNullArgument("ids");
            graphObject.Ids = $"ids:[{string.Join(',', ids.Select(id => $"\"{id}\""))}]";
            return this;
        }
        public TypeQueryBuilder<T> Locale(LocaleMode locale)
        {
            if (!locale.IsNull())
            {
                graphObject.Filter +=
                    graphObject.Filter.IsNullOrEmpty() ?
                    $"locale:{locale}" :
                    $",locale:{locale}";
            }
            return this;
        }
        public TypeQueryBuilder<T> Locales(params CultureInfo[] cultures)
        {
            cultures.ValidateNotNullArgument("cultures");
            string allLocales = string.Join(',', cultures.Select(c => c.Name.Replace("-", "_")));
            graphObject.Locale = allLocales;
            return this;
        }
        /// <summary>
        /// Order for a field. Add more for order many fields.
        /// </summary>
        /// <param name="fieldSelector">Field for order</param>
        /// <param name="orderMode">OrderMode</param>
        /// <returns></returns>
        public TypeQueryBuilder<T> OrderBy(Expression<Func<T, object>> fieldSelector, OrderMode orderMode = OrderMode.ASC)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var propertyName = fieldSelector.GetFieldPath();

            if (graphObject.OrderBy.IsNullOrEmpty())
            {
                graphObject.OrderBy = $"{propertyName}:{orderMode}";
            }
            else
            {
                graphObject.OrderBy += $",{propertyName}:{orderMode}";
            }
            return this;
        }
        public TypeQueryBuilder<T> OrderBy(Expression<Func<T, object>> fieldSelector, OrderMode orderMode, Ranking? ranking)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var propertyName = fieldSelector.GetFieldPath();

            if (graphObject.OrderBy.IsNullOrEmpty())
            {
                graphObject.OrderBy = $"{propertyName}:{orderMode},_ranking:{ranking}";
            }
            else
            {
                graphObject.OrderBy += $",{propertyName}:{orderMode},_ranking:{ranking}";
            }

            return this;
        }
        public TypeQueryBuilder<T> OrderBy(Ranking ranking = Ranking.SEMANTIC)
        {
            if (graphObject.OrderBy.IsNullOrEmpty())
            {
                graphObject.OrderBy = $"_ranking:{ranking}";
            }
            else
            {
                graphObject.OrderBy += $",_ranking:{ranking}";
            }

            return this;
        }
        public TypeQueryBuilder<T> Limit(int limit = 20)
        {
            graphObject.Limit = $"limit:{limit}";
            return this;
        }
        public TypeQueryBuilder<T> Search(IFilterOperator filterOperator)
        {
            Where(FIELDS.FULLTEXT, filterOperator);
            return this;
        }
        /// <summary>
        /// Full text search using Match operator
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> Search(string q)
        {
            Where(FIELDS.FULLTEXT, new StringFilterOperators().Match(q));
            return this;
        }
        private Dictionary<string, string> FullTextFilters = new Dictionary<string, string>();
        private string MergeFilters(string exsting, string additional)
        {
            Regex reg = new Regex(@"\{.*\}");
            var originalFilter = reg.Match(exsting).Value;
            var newFilter = reg.Match(additional).Value;
            if (originalFilter != newFilter)
            {
                return $"{originalFilter.Replace("{","").Replace("}","")},{newFilter.Replace("{", "").Replace("}", "")}";
            }
            return exsting;
        }
        public TypeQueryBuilder<T> Where(string fieldName, IFilterOperator filterOperator)
        {
            filterOperator.ValidateNotNullArgument("filterOperator");
            var combinedQuery = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldName, filterOperator);
            if (fieldName == FIELDS.FULLTEXT)
            {
                if (FullTextFilters.ContainsKey(fieldName))
                {
                    var existingFilter = FullTextFilters[fieldName];
                    var finalFilter = $"{fieldName}: {{{MergeFilters(existingFilter, combinedQuery)}}}";
                    graphObject.WhereClause = graphObject.WhereClause.Replace(existingFilter, "");
                    combinedQuery = finalFilter;
                    FullTextFilters.Remove(fieldName);
                    FullTextFilters.Add(fieldName, finalFilter);
                }
                else
                {
                    FullTextFilters.Add(fieldName, combinedQuery);
                }
            }

            if (graphObject.WhereClause.IsNullOrEmpty())
            {
                graphObject.WhereClause = $"{combinedQuery}";
            }
            else
            {
                graphObject.WhereClause += $",{combinedQuery}";
            }

            return this;
        }
        public TypeQueryBuilder<T> Where(Expression<Func<T, Filter>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            if (fieldSelector != null)
            {
                var paser = new FilterExpressionParser();
                var filter = paser.GetFilter(fieldSelector);
                Where(filter);
            }
            return this;
        }
        public TypeQueryBuilder<T> Where(Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            Where(fieldSelector.GetFieldPath(), filterOperator);

            return this;
        }
        public TypeQueryBuilder<T> Where(Expression<Func<T, long?>> fieldSelector, NumericFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            Where(fieldSelector.GetFieldPath(), filterOperator);

            return this;
        }
        public TypeQueryBuilder<T> Where(Expression<Func<T, string>> fieldSelector, DateFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            Where(fieldSelector.GetFieldPath(), filterOperator);

            return this;
        }
        public TypeQueryBuilder<T> Where(Expression<Func<T, bool>> fieldSelector, BooleanFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            Where(fieldSelector.GetFieldPath(), filterOperator);

            return this;
        }
        public TypeQueryBuilder<T> Where(Expression<Func<T, DateTime?>> fieldSelector, DateFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            Where(fieldSelector.GetFieldPath(), filterOperator);

            return this;
        }
        public TypeQueryBuilder<T> Where(IFilter filter)
        {
            filter.ValidateNotNullArgument("filter");
            if (graphObject.WhereClause.IsNullOrEmpty())
            {
                graphObject.WhereClause = $"{filter.FilterClause}";
            }
            else
            {
                graphObject.WhereClause += $",{filter.FilterClause}";
            }

            return this;
        }
        private TypeQueryBuilder<T> Facet(string propertyName, IFacetOperator facetFilter)
        {
            propertyName.ValidateNotNullArgument("propertyName");
            facetFilter.ValidateNotNullArgument("facetFilter");
            string facets = ConvertNestedFieldToString.ConvertNestedFieldForFacet(propertyName, facetFilter);
            graphObject.Facets = graphObject.Facets.IsNullOrEmpty() ?
                $"{facets}" :
                $"{graphObject.Facets} {facets}";
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, object>> fieldSelector, IFacetOperator facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            Facet(fieldSelector.GetFieldPath(), facetFilter);
            return this;
        }
        public TypeQueryBuilder<T> Facet(params Expression<Func<T, FacetFilter>>[] facetFilters)
        {
            facetFilters.ValidateNotNullArgument("facetFilters");
            var parser = new FacetExpressionParser();
            foreach (var facetFilter in facetFilters)
            {
                var filter = parser.GetFacetFilter(facetFilter);
                Facet(filter);
            }
            return this;
        }
        public TypeQueryBuilder<T> Facet(IEnumerable<FacetFilter> facetFilters)
        {
            facetFilters.ValidateNotNullArgument("facetFilters");
            foreach (var facetFilter in facetFilters)
            {
                Facet(facetFilter);
            }
            return this;
        }
        /// <summary>
        /// Get facet with filter
        /// </summary>
        /// <param name="fieldSelector"></param>
        /// <param name="facetFilter"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> Facet(Expression<Func<T, string>> fieldSelector, StringFacetFilterOperators facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            Facet(fieldSelector.GetFieldPath(), facetFilter);
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, bool>> fieldSelector, StringFacetFilterOperators facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            Facet(fieldSelector.GetFieldPath(), facetFilter);
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, object>> fieldSelector, StringFacetFilterOperators facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            Facet(fieldSelector.GetFieldPath(), facetFilter);
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, int?>> fieldSelector, NumericFacetFilterOperators facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            Facet(fieldSelector.GetFieldPath(), facetFilter);
            return this;
        }
        public TypeQueryBuilder<T> Facet(Expression<Func<T, DateTime?>> fieldSelector, DateFacetFilterOperators facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            Facet(fieldSelector.GetFieldPath(), facetFilter);
            return this;
        }
        public TypeQueryBuilder<T> Facet(IFacetFilter facetFilter)
        {
            facetFilter.ValidateNotNullArgument("facetFilter");
            graphObject.Facets = graphObject.Facets.IsNullOrEmpty() ?
                $"{facetFilter.FilterClause}" :
                $"{graphObject.Facets} {facetFilter.FilterClause}";
            return this;
        }
        public TypeQueryBuilder<T> FilterForVisitor(params IFilterForVisitor[] filterForVisitors)
        {
            if (filterForVisitors != null && filterForVisitors.Length > 0)
            {
                _filters = filterForVisitors;
            }
            else
            {
                _filters ??= ServiceLocator.Current?.GetAllInstances<IFilterForVisitor>();
            }

            if (_filters.IsNotNull())
            {
                foreach (var filter in _filters)
                {
                    filter.FilterForVisitor(this);
                }
            }
            return this;
        }
        #endregion

        /// <summary>
        /// Build the query for current type
        /// </summary>
        /// <returns></returns>
        public override GraphQueryBuilder ToQuery()
        {
            if (!_compiled)
            {
                if (graphObject.SelectItems.IsNullOrEmpty() && graphObject.Total.IsNullOrEmpty() && graphObject.Facets.IsNullOrEmpty() && graphObject.Autocomplete.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("You must select at least one of the values [Field(s), Facet(s), Total, Autocomplete(s)]");
                }
                graphObject.TypeName = typeof(T).Name;
                if (!graphObject.Skip.IsNullOrEmpty())
                {
                    graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? graphObject.Skip : $"{graphObject.Filter},{graphObject.Skip}";
                }
                if (!graphObject.Limit.IsNullOrEmpty())
                {
                    graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? graphObject.Limit : $"{graphObject.Filter},{graphObject.Limit}";
                }
                if (!graphObject.Ids.IsNullOrEmpty())
                {
                    graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? graphObject.Ids : $"{graphObject.Filter},{graphObject.Ids}";
                }
                if (!graphObject.SelectItems.IsNullOrEmpty())
                {
                    graphObject.SelectItems = $"items{{{graphObject.SelectItems}}}";
                }

                if (!graphObject.WhereClause.IsNullOrEmpty())
                {
                    if (graphObject.Filter.IsNullOrEmpty())
                    {
                        graphObject.WhereClause = $"where:{{{graphObject.WhereClause}}}";
                    }
                    else
                    {
                        graphObject.WhereClause = $",where:{{{graphObject.WhereClause}}}";
                    }
                }
                if (!graphObject.Locale.IsNullOrEmpty())
                {
                    graphObject.Locale = $"locale:[{graphObject.Locale}]";
                }
                if (!graphObject.OrderBy.IsNullOrEmpty())
                {
                    graphObject.OrderBy = $"orderBy:{{{graphObject.OrderBy}}}";
                    graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? graphObject.OrderBy : $"{graphObject.Filter},{graphObject.OrderBy}";
                }
                if (!graphObject.Locale.IsNullOrEmpty() || !graphObject.Filter.IsNullOrEmpty() || !graphObject.WhereClause.IsNullOrEmpty())
                {
                    graphObject.Filter = $"({graphObject.Locale}{graphObject.Filter}{graphObject.WhereClause})";
                }
                if (!graphObject.Facets.IsNullOrEmpty())
                {
                    graphObject.Facets = $"facets{{{graphObject.Facets}}}";
                }
                if (!graphObject.Autocomplete.IsNullOrEmpty())
                {
                    graphObject.Autocomplete = $"autocomplete{{{graphObject.Autocomplete}}}";
                }
                _query.Query = graphObject.ToString();
                _compiled = true;
                if (this.Parent != null)
                {
                    this.Parent.AddQuery(_query.Query);
                }
            }

            return this.Parent != null ? (GraphQueryBuilder)this.Parent : new GraphQueryBuilder(_query, this);
        }
    }

    public class TypeQueryBuilder : BaseTypeQueryBuilder
    {
        public TypeQueryBuilder(GraphQLRequest request) : base(request)
        {
        }
        public TypeQueryBuilder() : base()
        {
        }

    }
}
