
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ServiceLocation;

namespace EPiServer.ContentGraph.Api.Filters
{
    [ServiceConfiguration(typeof(IFilterForVisitor))]
    public class FilterDeletedForVisitor : IFilterForVisitor
    {
        public void FilterForVisitor<T>(TypeQueryBuilder<T> typeQueryBuilder)
        {
            typeQueryBuilder.ValidateNotNullArgument("typeQueryBuilder");
            typeQueryBuilder.Where("Status", new StringFilterOperators().Eq("Published"));
        }
    }
}
