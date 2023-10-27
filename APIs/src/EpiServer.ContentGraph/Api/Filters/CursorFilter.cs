namespace EPiServer.ContentGraph.Api.Filters
{
    public class CursorFilter : IFilter
    {
        public string FilterClause => $"cursor";
        public IList<IFilter> Filters { get; set; }
    }
}
