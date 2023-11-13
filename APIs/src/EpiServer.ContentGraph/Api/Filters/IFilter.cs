namespace EPiServer.ContentGraph.Api.Filters
{
    public interface IFilter
    {
        public string FilterClause { get; }
        public IList<IFilter> Filters { get; set; }
    }
}
