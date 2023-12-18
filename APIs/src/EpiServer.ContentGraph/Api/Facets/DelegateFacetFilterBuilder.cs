namespace EPiServer.ContentGraph.Api.Facets
{
    public class DelegateFacetFilterBuilder : FacetFilter
    {
        private Func<string, FacetFilter> filterDelegate;
        public DelegateFacetFilterBuilder(Func<string, FacetFilter> filterDelegate = null):base(string.Empty)
        {
            this.filterDelegate = filterDelegate;
        }
        public FacetFilter GetFacetFilter(string fieldName)
        {
            return filterDelegate(fieldName);
        }

        public IEnumerable<FacetProperty> FacetProjections => null;

    }
}
