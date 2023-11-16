using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class TermFacetFilter : IFacetFilter
    {
        string _field = string.Empty;
        IFacetOperator _facetOperator;
        public TermFacetFilter(string field, IFacetOperator facetOperator)
        {
            _field = field;
            _facetOperator = facetOperator;
        }

        public string FilterClause => ConvertNestedFieldToString.ConvertNestedFieldForFacet(_field, _facetOperator);
    }
}
