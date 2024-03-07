using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class TermFacetFilter : FacetFilter
    {
        string _field = string.Empty;
        IFacetOperator _facetOperator;
        public TermFacetFilter(string field, IFacetOperator facetOperator):base(string.Empty)
        {
            _field = field;
            _facetOperator = facetOperator;
        }

        public override string FilterClause => ConvertNestedFieldToString.ConvertNestedFieldForFacet(_field, _facetOperator);
    }
}
