namespace EPiServer.ContentGraph.Api.Facets
{
    public class DelegateFacetFilterBuilder : IFacetFilter
    {
        private Func<string, IFacetFilter> filterDelegate;
        private string filterName;
        public DelegateFacetFilterBuilder(Func<string, IFacetFilter> filterDelegate = null)
        {
            this.filterDelegate = filterDelegate;
            filterName = string.Empty;
        }
        public IFacetFilter GetFacetFilter(string fieldName)
        {
            this.filterName = fieldName;
            return filterDelegate(fieldName);
        }
        public string GetCurrentField()
        {
            return filterName;
        }
        public string FilterClause => string.Empty;

        public IEnumerable<FacetProperty> FacetProjections => null;
    }
}
