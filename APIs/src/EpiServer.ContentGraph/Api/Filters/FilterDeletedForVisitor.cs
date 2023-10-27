
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    //[ServiceConfiguration(typeof(IFilterForVisitor))]
    public class FilterDeletedForVisitor : IFilterForVisitor
    {
        public void FilterForVisitor<T>(TypeQueryBuilder<T> typeQueryBuilder) //where T : IContentData
        {
            typeQueryBuilder.ValidateNotNullArgument("typeQueryBuilder");
            typeQueryBuilder.Where("Status", new StringFilterOperators().NotEq("Deleted"));
        }
    }
}
