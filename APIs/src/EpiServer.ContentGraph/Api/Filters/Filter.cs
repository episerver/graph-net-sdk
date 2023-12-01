namespace EPiServer.ContentGraph.Api.Filters
{
    public abstract class Filter : IFilter
    {
        protected IList<IFilter>? _filters;
        public virtual string FilterClause => string.Empty;
        protected virtual void AddFilter(IFilter filter)
        {
            _filters ??= new List<IFilter>();
            _filters.Add(filter);
        }
        public IEnumerable<IFilter> GetFilters()
        {
           return _filters ?? new List<IFilter>();
        }
        public Filter()
        {
            _filters = new List<IFilter>();
        }
        public static Filter operator &(Filter first, Filter second)
        {
            AndFilter andFilter = new AndFilter();
            andFilter.AddFilter(first);
            andFilter.AddFilter(second);
            return andFilter;
        }
        public static Filter operator |(Filter first, Filter second)
        {
            OrFilter orFilter = new OrFilter();
            orFilter.AddFilter(first);
            orFilter.AddFilter(second);
            return orFilter;
        }
        public static Filter operator !(Filter first)
        {
            NotFilter notFilter = new NotFilter();
            notFilter.AddFilter(first);
            return notFilter;
        }
    }
}
