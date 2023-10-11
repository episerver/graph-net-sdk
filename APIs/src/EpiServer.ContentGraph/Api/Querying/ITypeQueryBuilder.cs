using GraphQL.Transport;


namespace EPiServer.ContentGraph.Api.Querying
{
    public interface ITypeQueryBuilder : IQuery
    {
        public GraphQLRequest GetQuery();
        public GraphQueryBuilder ToQuery();
    }
}
