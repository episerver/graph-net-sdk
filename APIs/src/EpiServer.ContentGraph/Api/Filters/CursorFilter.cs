namespace EPiServer.ContentGraph.Api.Filters
{
    public class CursorFilter : IFilter
    {
        public string FilterClause => $"cursor";
        public List<IFilter> Filters { get; set; }
    }
}
