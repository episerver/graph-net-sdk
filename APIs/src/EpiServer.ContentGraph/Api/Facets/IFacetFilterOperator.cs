namespace EPiServer.ContentGraph.Api.Facets
{
    public interface IFacetFilterOperator
    {
        public string Query { get;}
        public IEnumerable<FacetProjection> FacetProjections { get;}
    }
}
