using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using System;

namespace AlloyMvcTemplates
{
    public class FilterForModified : IFilterForVisitor
    {
        public void FilterForVisitor<T>(TypeQueryBuilder<T> typeQueryBuilder)
        {
            typeQueryBuilder.Where("_modified", new DateFilterOperators().Gte(DateTime.Now.ToUniversalTime().AddMinutes(-5).ToString("s") + "Z"));
        }
    }
}
