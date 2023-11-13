namespace EPiServer.ContentGraph.Api.Filters
{
    public abstract class Filter : IFilter
    {
        private IList<IFilter>? _filters;
        public virtual string FilterClause => string.Empty;

        public IList<IFilter> Filters
        {
            get
            {
                _filters ??= new List<IFilter>();
                return _filters;
            }
            set => _filters = value;
        }
        public static Filter operator &(Filter first, Filter second)
        {
            AndFilter andFilter = new AndFilter();
            andFilter.Filters.Add(first);
            andFilter.Filters.Add(second);
            return andFilter;
        }
        public static Filter operator |(Filter first, Filter second)
        {
            OrFilter orFilter = new OrFilter();
            orFilter.Filters.Add(first);
            orFilter.Filters.Add(second);
            return orFilter;
        }
        public static Filter operator !(Filter first)
        {
            NotFilter notFilter = new NotFilter();
            notFilter.Filters.Add(first);
            return notFilter;
        }
    }
}
