
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ServiceLocation;

namespace EPiServer.ContentGraph.Api.Filters
{
    [ServiceConfiguration(typeof(IFilterForVisitor), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CustomForVisitor : IFilterForVisitor
    {
        public void FilterForVisitor<T>(TypeQueryBuilder<T> typeQueryBuilder) //where T : IContentData
        {
            typeQueryBuilder.ValidateNotNullArgument("typeQueryBuilder");
            typeQueryBuilder.Where("Author", new StringFilterOperators().Eq("optiq"));
        }
    }
}
