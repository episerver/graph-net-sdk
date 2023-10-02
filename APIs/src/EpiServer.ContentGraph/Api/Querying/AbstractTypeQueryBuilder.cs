using EPiServer.ContentGraph.Api.Result;
using GraphQL.Transport;

namespace EPiServer.ContentGraph.Api.Querying
{
    public abstract class AbstractTypeQueryBuilder : ITypeQueryBuilder
    {
        protected readonly ContentGraphQuery graphObject;
        protected readonly GraphQLRequest _query;
        public AbstractTypeQueryBuilder()
        {
            graphObject = new ContentGraphQuery();
            _query = new GraphQLRequest();
        }
        public AbstractTypeQueryBuilder(GraphQLRequest query)
        {
            graphObject = new ContentGraphQuery();
            _query = query;
        }

        public virtual GraphQueryBuilder Build()
        {
            _query.Query = graphObject.ToString();
            return new GraphQueryBuilder(_query);
        }

        public virtual GraphQLRequest GetQuery()
        {
            return _query;
        }

        public ContentGraphResult<TResult> GetResult<TResult>()
        {
            throw new NotImplementedException();
        }
    }
}
