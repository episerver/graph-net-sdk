using EPiServer.ContentGraph.Api.Result;
using GraphQL.Transport;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class BaseTypeQueryBuilder : ITypeQueryBuilder
    {
        protected readonly ContentGraphQuery graphObject;
        protected readonly GraphQLRequest _query;
        public BaseTypeQueryBuilder()
        {
            graphObject = new ContentGraphQuery();
            _query = new GraphQLRequest();
        }
        public BaseTypeQueryBuilder(GraphQLRequest query)
        {
            graphObject = new ContentGraphQuery();
            _query = query;
        }

        public virtual GraphQueryBuilder ToQuery()
        {
            _query.Query = graphObject.ToString();
            return new GraphQueryBuilder(_query);
        }

        public virtual GraphQLRequest GetQuery()
        {
            return _query;
        }
    }
}
