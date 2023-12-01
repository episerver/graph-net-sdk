using GraphQL.Transport;


namespace EPiServer.ContentGraph.Api.Querying
{
    public interface ITypeQueryBuilder
    {
        public IQuery Parent { get; set; }
        public GraphQLRequest GetQuery();
        public GraphQueryBuilder ToQuery();
    }
}
