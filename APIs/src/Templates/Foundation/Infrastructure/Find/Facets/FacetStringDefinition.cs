//using EPiServer.Find;
//using EPiServer.Find.Api.Facets;
//using EPiServer.Find.Api.Querying;

using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Api.Result;

namespace Foundation.Infrastructure.Find.Facets
{
    public class FacetStringDefinition : FacetDefinition
    {
        public FacetStringDefinition() => RenderType = GetType().Name;

        public TypeQueryBuilder<T> Filter<T>(TypeQueryBuilder<T> query, List<string> selectedValues) => selectedValues.Any() ? query.Where(FieldName, new StringFilterOperators().In(selectedValues.ToArray())) : query;

        public override TypeQueryBuilder<T> Facet<T>(TypeQueryBuilder<T> query, IFacetOperator filter) => query.Facet(new TermFacetFilter(FieldName, filter));

        public override void PopulateFacet(FacetGroupOption facetGroupOption, IEnumerable<Facet> facet, string selectedFacets)
        {
            if (facet == null)
            {
                return;
            }
            facetGroupOption.Facets = facet.Select(x =>
            {
                var ranges = x.Name.Replace("(", "").Replace(")", "").Replace("[", "'").Replace("]", "").Split(',');
                double? from = null, to = null;

                if (ranges?.Length > 1)
                {
                    to = double.Parse(ranges[1]);

                }
                if (ranges?.Length > 0)
                {
                    from = double.Parse(ranges[0]);
                }

                return new FacetOption()
                {
                    Count = x.Count,
                    Key = x.Name,
                    Name = x.Name,
                    Selected = selectedFacets != null && selectedFacets.Contains($"{x.Name}:{x.Count}")
                };

            }).ToList();
        }
    }
}