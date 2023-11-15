namespace EPiServer.ContentGraph.Api.Filters
{
    public class DelegateFilterBuilder : Filter
    {
        private Func<string, Filter> filterDelegate;
        public DelegateFilterBuilder(Func<string, Filter> filterDelegate = null)
        {
            this.filterDelegate = filterDelegate;
        }

        public Filter GetFilter(string fieldName)
        {
            return filterDelegate(fieldName);
        }
    }
}
