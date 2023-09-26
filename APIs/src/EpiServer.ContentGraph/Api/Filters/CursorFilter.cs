namespace EPiServer.ContentGraph.Api.Filters
{
    public class CursorFilter : IGraphFilter
    {
        public string FilterClause => $"cursor";
    }
}
