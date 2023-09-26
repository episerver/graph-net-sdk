using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Helpers.Reflection;
using GraphQL.Transport;
using System.Linq.Expressions;
using System.Globalization;
using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;
using System.Text;

namespace EPiServer.ContentGraph.Api.Querying
{
    //TODO: Very important=> remove all quotes, prefix wilcard and script-injection for security
    public class TypeQueryBuilder<T> : ITypeQueryBuilder
    {
        private readonly ContentGraphQuery graphObject;
        private readonly GraphQLRequest _query;
        public TypeQueryBuilder(GraphQLRequest request)
        {
            _query = request;
            graphObject = new ContentGraphQuery();
        }
        public TypeQueryBuilder()
        {
            _query = new GraphQLRequest();
            graphObject = new ContentGraphQuery();
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
        public TypeQueryBuilder<T> Field(string propertyName)
        {
            graphObject.SelectItems = string.Concat(graphObject.SelectItems, $" {ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}");
            return this;
        }
        public TypeQueryBuilder<T> ForSubType<TSub>(string propertyName)
        {
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = string.Concat(graphObject.SelectItems, $"... on {subTypeName}{{{ConvertNestedFieldToString.ConvertNestedFieldForQuery(propertyName)}}}");
            return this;
        }
        /// <summary>
        /// Select field for a sub type
        /// </summary>
        /// <typeparam name="TSub"></typeparam>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> ForSubType<TSub>(Expression<Func<TSub, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var propertyName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(fieldSelector.GetFieldPath());
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = string.Concat(graphObject.SelectItems, $"... on {subTypeName}{{{propertyName}}}");
            return this;
        }
        public TypeQueryBuilder<T> ForSubType<TSub>(SubTypeQueryBuilder<TSub> subTypeQuery)
        {
            subTypeQuery.ValidateNotNullArgument("subTypeQuery");
            string subTypeName = typeof(TSub).Name;
            graphObject.SelectItems = string.Concat(graphObject.SelectItems, $"... on {subTypeName}{{{subTypeQuery.Query}}}");
            return this;
        }
        public TypeQueryBuilder<T> Skip(int skip)
        {
            if (graphObject.Filter.IsNullOrEmpty())
            {
                graphObject.Filter = string.Concat(graphObject.Filter, "skip:" + skip);
            }
            else
            {
                graphObject.Filter = string.Concat(graphObject.Filter, ",skip:" + skip);
            }
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
                    string.Concat(graphObject.Filter, $"cursor:\"{scrollId}\"") :
                    string.Concat(graphObject.Filter, $",cursor:\"{scrollId}\"");
            }
            return this;
        }
        public TypeQueryBuilder<T> Ids(string[] ids)
        {
            if (graphObject.Filter.IsNullOrEmpty())
            {
                graphObject.Filter = string.Concat(graphObject.Filter, $"ids:[{string.Concat(',', ids)}]");
            }
            else
            {
                graphObject.Filter = string.Concat(graphObject.Filter, $",ids:[{string.Concat(',', ids)}]");
            }
            return this;
        }
        /// <summary>
        /// Set locale for query. Currently not support localization culture
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
        public TypeQueryBuilder<T> Limit(int limit)
        {
            if (graphObject.Filter.IsNullOrEmpty())
            {
                graphObject.Filter = string.Concat(graphObject.Filter, "limit:" + limit);
                
            }
            else
            {
                graphObject.Filter = string.Concat(graphObject.Filter, ",limit:" + limit);
            }
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
        public TypeQueryBuilder<T> Where(Expression<Func<T, object>> fieldSelector, IFilterOperator filterOperator)
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
        /// order only one field , it's not hard to implement order for many fields
        /// </summary>
        /// <param name="fieldSelector"></param>
        /// <param name="orderMode"></param>
        /// <returns></returns>
        public TypeQueryBuilder<T> OrderBy(Expression<Func<T, object>> fieldSelector, OrderMode orderMode = OrderMode.ASC)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var propertyName = fieldSelector.GetFieldPath();

            if (graphObject.Filter.IsNullOrEmpty())
            {
                graphObject.Filter = $"orderBy:{{{propertyName}:{orderMode}}}";
            }
            else
            {
                graphObject.Filter += $",orderBy:{{{propertyName}:{orderMode}}}";
            }

            return this;
        }
        /// <summary>
        /// Build the query for current type
        /// </summary>
        /// <returns></returns>
        public GraphQueryBuilder Build()
        {
            graphObject.TypeName = typeof(T).Name;
            graphObject.SelectItems = $"items {{{graphObject.SelectItems}}}";

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

            if (!graphObject.Filter.IsNullOrEmpty() || !graphObject.WhereClause.IsNullOrEmpty())
            {
                graphObject.Filter = $"({graphObject.Filter}{graphObject.WhereClause})";
            }
            if (!graphObject.Facets.IsNullOrEmpty())
            {
                graphObject.Facets = $"facets {{{graphObject.Facets}}}";
            }
            if (!graphObject.Autocomplete.IsNullOrEmpty())
            {
                graphObject.Autocomplete = $"autocomplete {{{graphObject.Autocomplete}}}";
            }
            _query.Query = graphObject.ToString();
            return new GraphQueryBuilder(_query);
        }
        public GraphQLRequest GetQuery()
        {
            return _query;
        }
        #region Queries
        public TypeQueryBuilder<T> Facets(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            fieldSelector.Compile();
            var propertyName = ConvertNestedFieldToString.ConvertNestedFieldForQuery(fieldSelector.GetFieldPath());
            //TODO: remove fixed prop name+count, it should come from fieldSelector
            graphObject.Facets += $"{propertyName}{{{"name count"}}}";
            return this;
        }
        public TypeQueryBuilder<T> Facets(string propertyName)
        {
            //TODO: remove fixed prop name+count, it should come from fieldSelector
            graphObject.Facets += $"{propertyName}{{{"name count"}}}";
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
                graphObject.Total = $"total(all:{isAll})";
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
        public TypeQueryBuilder<T> Facets(Expression<Func<T, object>> fieldSelector, FacetFilter facetFilter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            facetFilter.ValidateNotNullArgument("facetFilter");
            fieldSelector.Compile();
            var propertyName = fieldSelector.GetFieldPath();
            //TODO: remove fixed prop name+count, it should come from fieldSelector
            graphObject.Facets += $"{propertyName}({facetFilter.Query}) {{{"name count"}}}";
            return this;
        }
        #endregion
    }
}
