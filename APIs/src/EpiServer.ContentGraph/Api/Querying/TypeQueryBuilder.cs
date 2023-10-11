using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL.Transport;
using System.Linq.Expressions;
using System.Globalization;
using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;

namespace EPiServer.ContentGraph.Api.Querying
{
    //TODO: Very important=> remove all quotes, prefix wilcard and script-injection for security
    public class TypeQueryBuilder<T> : AbstractTypeQueryBuilder
    {
        public TypeQueryBuilder(GraphQLRequest request): base(request)
        {  
        }
        public TypeQueryBuilder():base()
        {
        }
        public TypeQueryBuilder<T> Field(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var propertyName = fieldSelector.GetFieldPath();
            if (graphObject.SelectItems.IsNullOrEmpty())
            {
                graphObject.SelectItems = $"{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}";
            }
            else
            {
                graphObject.SelectItems += $" {ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}";
            }
            
            return this;
        }
        public TypeQueryBuilder<T> Fields(params Expression<Func<T, object>>[] fieldSelectors)
        {
            fieldSelectors.ValidateNotNullArgument("fieldSelectors");
            foreach (var fieldSelector in fieldSelectors)
            {
                fieldSelector.ValidateNotNullArgument("fieldSelector");
                fieldSelector.Compile();
                var propertyName = fieldSelector.GetFieldPath();
                if (graphObject.SelectItems.IsNullOrEmpty())
                {
                    graphObject.SelectItems = $"{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}";
                }
                else
                {
                    graphObject.SelectItems += $" {ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}";
                }
            }
            return this;
        }
        public TypeQueryBuilder<T> Field(string propertyName)
        {
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}" :
                $"{graphObject.SelectItems} {ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}";
            return this;
        }
        /// <summary>
        /// Select fields in a subtype
        /// </summary>
        /// <typeparam name="TSub">Subtype must be inherited from type <typeparamref name="T"/></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> ForSubType<TSub>(string propertyName) where TSub : T
        {
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"... on {subTypeName}{{{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}}}" :
                $"{graphObject.SelectItems} ... on {subTypeName}{{{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}}}";
            return this;
        }
        /// <summary>
        /// Select fields in a subtype
        /// </summary>
        /// <typeparam name="TSub">Subtype must be inherited from type <typeparamref name="T"/></typeparam>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> ForSubType<TSub>(Expression<Func<TSub, object>> fieldSelector) where TSub:T
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            //if (!typeof(TSub).IsAssignableTo(typeof(T)))
            //{
            //    throw new ArgumentException($"Type [{typeof(TSub).Name}] is not inherit from type [{typeof(T).Name}]");
            //}
            fieldSelector.Compile();
            var propertyName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(fieldSelector.GetFieldPath());
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"... on {subTypeName}{{{propertyName}}}" :
                $"{graphObject.SelectItems} ... on {subTypeName}{{{propertyName}}}";
            return this;
        }
        /// <summary>
        /// Select fields in a subtype
        /// </summary>
        /// <typeparam name="TSub">Subtype must be inherited from type <typeparamref name="T"/></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> ForSubType<TSub>(SubTypeQueryBuilder<TSub> subTypeQuery) where TSub : T
        {
            subTypeQuery.ValidateNotNullArgument("subTypeQuery");
            //if (!typeof(TSub).IsAssignableTo(typeof(T)))
            //{
            //    throw new ArgumentException($"Type [{typeof(TSub).Name}] is not inherit from type [{typeof(T).Name}]");
            //}
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = graphObject.SelectItems.IsNullOrEmpty() ?
                $"... on {subTypeName}{{{subTypeQuery.Query}}}" :
                $"{graphObject.SelectItems} ... on {subTypeName}{{{subTypeQuery.Query}}}";
            return this;
        }
        public TypeQueryBuilder<T> Skip(long skip)
        {
            graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? 
                $"skip:{skip}" : 
                $"{graphObject.Filter},skip:{skip}";
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
            graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? 
                $"ids:[{string.Join(',', ids.Select(id => $"\"{id}\""))}]" : 
                $"{graphObject.Filter},ids:[{string.Join(',', ids.Select(id=>$"\"{id}\""))}]";
            return this;
        }
        /// <summary>
        /// Set locale for query. Currently not support localization culture.
        /// </summary>
        /// <param name="language">Culture for query, if null it will be ALL cultures</param>
        /// <returns>TypeQueryBuilder</returns>
        public TypeQueryBuilder<T> Locale(CultureInfo? language)
        {
            if (language.IsNull())
            {
                graphObject.Filter +=
                    graphObject.Filter.IsNullOrEmpty() ?
                    $"locale:{LocaleMode.ALL}" :
                    $",locale:{LocaleMode.ALL}";
            }
            else
            {
                graphObject.Filter +=
                    graphObject.Filter.IsNullOrEmpty() ?
                    $"locale:{language.TwoLetterISOLanguageName.ToLower()}" :
                    $",locale:{language.TwoLetterISOLanguageName.ToLower()}";
            }
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
        public TypeQueryBuilder<T> Locale(string culture)
        {
            try
            {
                CultureInfo.GetCultureInfo(culture);
                graphObject.Filter +=
                    graphObject.Filter.IsNullOrEmpty() ?
                    $"locale:{culture}" :
                    $",locale:{culture}";
            }
            catch (CultureNotFoundException ex)
            {
                throw ex;
            }
            return this;
        }
        public TypeQueryBuilder<T> Limit(long limit)
        {
            graphObject.Filter = graphObject.Filter.IsNullOrEmpty() ? 
                $"limit:{limit}" :
                $"{graphObject.Filter},limit:{limit}";
            return this;
        }
        public TypeQueryBuilder<T> FullTextSearch(IFilterOperator filterOperator)
        {
            Where("_fulltext", filterOperator);
            return this;
        }
        private TypeQueryBuilder<T> Where(string fieldName, IFilterOperator filterOperator)
        {
            filterOperator.ValidateNotNullArgument("filterOperator");
            var combinedQuery = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldName, filterOperator);

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
        public TypeQueryBuilder<T> Where(Expression<Func<T, string>> fieldSelector, StringFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            fieldSelector.Compile();
            var combinedQuery = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);

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
        public TypeQueryBuilder<T> Where(Expression<Func<T, long?>> fieldSelector, NumericFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            fieldSelector.Compile();
            var combinedQuery = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);

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
        public TypeQueryBuilder<T> Where(Expression<Func<T, string>> fieldSelector, DateFilterOperators filterOperator)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            filterOperator.ValidateNotNullArgument("filterOperator");
            fieldSelector.Compile();
            var combinedQuery = ConvertNestedFieldToString.ConvertNestedFieldFilter(fieldSelector.GetFieldPath(), filterOperator);

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
        public TypeQueryBuilder<T> Where(IGraphFilter booleanFilter)
        {
            booleanFilter.ValidateNotNullArgument("booleanFilter");
            if (graphObject.WhereClause.IsNullOrEmpty())
            {
                graphObject.WhereClause = $"{booleanFilter.FilterClause}";
            }
            else
            {
                graphObject.WhereClause += $",{booleanFilter.FilterClause}";
            }

            return this;
        }
        public TypeQueryBuilder<T> Autocomplete(Expression<Func<T, string>> fieldSelector, AutoCompleteOperators autocomplete)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            autocomplete.ValidateNotNullArgument("autocompleteOperator");
            fieldSelector.Compile();
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
        /// Order for a field. Add more for order many fields.
        /// </summary>
        /// <param name="fieldSelector">Field for order</param>
        /// <param name="orderMode">OrderMode</param>
        /// <returns></returns>
        public TypeQueryBuilder<T> OrderBy(Expression<Func<T, object>> fieldSelector, OrderMode orderMode = OrderMode.ASC)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var propertyName = fieldSelector.GetFieldPath();

            if (graphObject.OrderBy.IsNullOrEmpty())
            {
                graphObject.OrderBy = $"orderBy:{{{propertyName}:{orderMode}}}";
            }
            else
            {
                graphObject.OrderBy += $",orderBy:{{{propertyName}:{orderMode}}}";
            }

            return this;
        }
        /// <summary>
        /// Build the query for current type
        /// </summary>
        /// <returns></returns>
        public override GraphQueryBuilder ToQuery()
        {
            if (graphObject.SelectItems.IsNullOrEmpty() && graphObject.Total.IsNullOrEmpty() && graphObject.Facets.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Can not build query with none of field");
            }
            graphObject.TypeName = typeof(T).Name;

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

            if (!graphObject.Filter.IsNullOrEmpty() || !graphObject.WhereClause.IsNullOrEmpty() || !graphObject.OrderBy.IsNullOrEmpty())
            {
                graphObject.Filter = $"({graphObject.Filter}{graphObject.WhereClause}{graphObject.OrderBy})";
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
            return new GraphQueryBuilder(_query);
        }
        public override GraphQLRequest GetQuery()
        {
            return _query;
        }
        #region Queries
        /// <summary>
        /// Simple facet for one field
        /// </summary>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> Facet(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var facet = ConvertNestedFieldToString.ConvertNestedFieldForFacet(fieldSelector.GetFieldPath());
            graphObject.Facets = graphObject.Facets.IsNullOrEmpty() ? 
                $"{facet}" : 
                $"{graphObject.Facets} {facet}";
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
        /// <summary>
        /// Facets with facet filters inside
        /// </summary>
        /// <param name="fieldSelector"></param>
        /// <param name="facetFilter"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> Facet(Expression<Func<T, object>> fieldSelector, FacetFilter facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            fieldSelector.Compile();
            var propertyName = fieldSelector.GetFieldPath();
            string facets = ConvertNestedFieldToString.ConvertNestedFieldForFacet(propertyName, facetFilter);
            //TODO: remove fixed prop name+count, it should come from fieldSelector
            graphObject.Facets = graphObject.Facets.IsNullOrEmpty() ?
                $"{facets}" :
                $"{graphObject.Facets} {facets}";
            return this;
        }
        #endregion
    }
}
