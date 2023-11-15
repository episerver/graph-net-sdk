using EPiServer.ContentGraph.Api.Result;

namespace EPiServer.ContentGraph.Api.Querying
{
    public interface IQuery
    {
        public Task<ContentGraphResult<TResult>> GetResult<TResult>();
    }
}
