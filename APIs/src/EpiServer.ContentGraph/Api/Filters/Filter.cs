namespace EPiServer.ContentGraph.Api.Filters
{
    public abstract class Filter<T> : IFilter
    {
        private IList<IFilter>? _filters;
        public virtual string FilterClause => string.Empty;

        public IList<IFilter> Filters { get
            {
                if (_filters is null)
                {
                    _filters = new List<IFilter>();
                }
                return _filters;
            }
            set => _filters = value; 
        }
        public static Filter<T> operator &(Filter<T> first, Filter<T> second)
        {
            AndFilter<T> andFilter = new AndFilter<T>();
            andFilter.Filters.Add(first);
            andFilter.Filters.Add(second);
            return andFilter;
        }
        public static Filter<T> operator |(Filter<T> first, Filter<T> second)
        {
            OrFilter<T> orFilter = new OrFilter<T>();
            orFilter.Filters.Add(first);
            orFilter.Filters.Add(second);
            return orFilter;
        }
        public static Filter<T> operator !(Filter<T> first)
        {
            NotFilter<T> notFilter = new NotFilter<T>();
            notFilter.Filters.Add(first);
            return notFilter;
        }
    }
}
