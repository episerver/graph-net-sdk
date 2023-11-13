using GraphQL.Transport;


namespace EPiServer.ContentGraph.Api.Querying
{
    public interface ITypeQueryBuilder
    {
        public GraphQLRequest GetQuery();
        public GraphQueryBuilder ToQuery();
    }
}
