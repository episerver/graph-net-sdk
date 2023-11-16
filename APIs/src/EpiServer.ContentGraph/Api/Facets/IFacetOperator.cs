namespace EPiServer.ContentGraph.Api.Facets
{
    public interface IFacetOperator
    {
        public string FilterClause { get;}
        public IEnumerable<FacetProperty> FacetProjections { get; }
    }
}
