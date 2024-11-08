using EPiServer.ContentGraph.Api.Result;
using System.Threading.Tasks;

namespace EPiServer.ContentGraph.Api.Querying
{
    public interface IQuery
    {
        public Task<ContentGraphResult<TResult>> GetResultAsync<TResult>();
        public Task<ContentGraphResult> GetResultAsync();
        internal void AddFragment(FragmentBuilder fragmentBuilder);
        internal bool HasFragment(string fragmentName);
        internal void AddQuery(string typeQuery);
    }
}
