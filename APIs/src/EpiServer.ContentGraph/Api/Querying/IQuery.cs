using EPiServer.ContentGraph.Api.Result;

namespace EPiServer.ContentGraph.Api.Querying
{
    public interface IQuery
    {
        public ContentGraphResult<TResult> GetResult<TResult>();
    }
}
