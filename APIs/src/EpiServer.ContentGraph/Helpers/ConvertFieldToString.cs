using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;

namespace EPiServer.ContentGraph.Helpers
{
    public static class ConvertNestedFieldToString
    {
        const string facetProperties = "name count";
        public static string ConvertNestedFieldForQuery(string fieldName)
        {
            var nestedObjest = new List<string>(fieldName.Split("."));
            nestedObjest.Reverse();
            string combined = string.Empty;
            foreach (var field in nestedObjest)
            {
                if (combined.IsNullOrEmpty())
                {
                    combined = $"{field}";
                }
                else
                {
                    combined = $"{field}" + $"{{{combined}}}";
                }
            }
            return combined;
        }
        public static string ConvertNestedFieldFilter(string fieldName, IFilterOperator filterOperator)
        {
            var nestedObjest = new List<string>(fieldName.Split("."));
            nestedObjest.Reverse();
            string combined = string.Empty;
            foreach (var field in nestedObjest)
            {
                if (combined.IsNullOrEmpty())
                {
                    combined = $"{field}:{{{filterOperator.Query}}}";
                }
                else
                {
                    combined = $"{field}:" + $"{{{combined}}}";
                }
            }
            return combined;
        }
        public static string ConvertNestedFieldForAutoComplete(string fieldName, AutoCompleteOperators autoCompleteOperator)
        {
            var nestedObject = new List<string>(fieldName.Split("."));
            nestedObject.Reverse();
            string combined = string.Empty;
            foreach (var field in nestedObject)
            {
                if (combined.IsNullOrEmpty())
                {
                    combined = $"{field}({autoCompleteOperator.Query})";
                }
                else
                {
                    combined = $"{field}" + $"{{{combined}}}";
                }
            }
            return combined;
        }
        public static string ConvertNestedFieldForFacet(string fieldName)
        {
            var nestedObject = new List<string>(fieldName.Split("."));
            nestedObject.Reverse();
            string combined = string.Empty;
            foreach (var field in nestedObject)
            {
                if (combined.IsNullOrEmpty())
                {
                    combined = $"{field}{{{facetProperties}}}";
                }
                else
                {
                    combined = $"{field}{{{combined}}}";
                }
            }
            return combined;
        }
        public static string ConvertNestedFieldForFacet(string fieldName, IFacetFilterOperator facetFilter)
        {
            string facetProperties = string.Join(' ', facetFilter.FacetProjections);
            var nestedObject = new List<string>(fieldName.Split("."));
            nestedObject.Reverse();
            string combined = string.Empty;
            foreach (var field in nestedObject)
            {
                if (combined.IsNullOrEmpty())
                {
                    combined = $"{field}({facetFilter.Query}){{{facetProperties}}}";
                }
                else
                {
                    combined = $"{field}{{{combined}}}";
                }
            }
            return combined;
        }
    }
}
