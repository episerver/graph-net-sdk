using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Api.Result;

namespace Foundation.Infrastructure.Find.Facets
{
    public class FacetStringListDefinition : FacetDefinition
    {
        public FacetStringListDefinition()
        {
            RenderType = GetType().Name;
            Name = "Facet Test";
            DisplayName = "Facet Test Display Name";
        }

        public TypeQueryBuilder<T> Filter<T>(TypeQueryBuilder<T> query, List<string> selectedValues) => selectedValues.Any() ? query.Where(FieldName, new StringFilterOperators().In(selectedValues.ToArray())) : query;

        public override TypeQueryBuilder<T> Facet<T>(TypeQueryBuilder<T> query, IFacetOperator filter) => query.Facet(new TermFacetFilter(FieldName, filter));

        public override void PopulateFacet(FacetGroupOption facetGroupOption, IEnumerable<Facet> termsFacet, string selectedFacets)
        {
            if (termsFacet == null)
            {
                return;
            }

            facetGroupOption.Facets = termsFacet.Select(x => new FacetOption
            {
                Count = x.Count,
                Key = $"{x.Name}:{x.Count}",
                Name = x.Name,
                Selected = selectedFacets != null && selectedFacets.Contains($"{x.Name}:{x.Count}")
            }).ToList();
        }
    }
}