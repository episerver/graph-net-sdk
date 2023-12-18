
namespace EPiServer.ContentGraph.Api.Facets
{
    public class FacetFilter : IFacetFilter
    {
        string _filterClause = string.Empty;
        public FacetFilter(string filterClause)
        {
            _filterClause = filterClause;
        }
        public virtual string FilterClause => _filterClause;
        public static FacetFilter operator &(FacetFilter first, FacetFilter second)
        {
            return new FacetFilter($"{first.FilterClause},{second.FilterClause}");
        }
    }
}
