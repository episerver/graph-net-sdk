using EPiServer.ContentGraph.Api.Querying;

namespace EPiServer.ContentGraph.Api.Filters
{
    public interface IFilterForVisitor
    {
        public void FilterForVisitor<T>(TypeQueryBuilder<T> typeQueryBuilder); //where T : IContentData;
    }
}
