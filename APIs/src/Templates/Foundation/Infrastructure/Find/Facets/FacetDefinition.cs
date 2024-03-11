using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Api.Result;

namespace Foundation.Infrastructure.Find.Facets
{
    public class FacetDefinition
    {
        private string _displayName;

        public string Name { get; set; }

        public string DisplayName
        {
            get => !string.IsNullOrEmpty(FieldName)
                ? LocalizationService.Current.GetString("/facetregistry/" + FieldName.ToLowerInvariant(),
                    !string.IsNullOrEmpty(_displayName) ? _displayName : FieldName)
                : _displayName;

            set => _displayName = value;
        }

        public string FieldName { get; set; }
        public string RenderType { get; set; }

        public virtual TypeQueryBuilder<T> Facet<T>(TypeQueryBuilder<T> query, IFacetOperator op) => query.Facet(new TermFacetFilter(FieldName, op));
        public virtual void PopulateFacet(FacetGroupOption facetGroupOption, IEnumerable<Facet> facet, string selectedFacets) { }
    }
}