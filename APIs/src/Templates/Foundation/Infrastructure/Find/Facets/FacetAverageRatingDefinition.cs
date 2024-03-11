//using EPiServer.Find;
//using EPiServer.Find.Api.Facets;
//using EPiServer.Find.Api.Querying;

using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Api.Result;

namespace Foundation.Infrastructure.Find.Facets
{
    public class FacetAverageRatingDefinition : FacetDefinition
    {
        private readonly ICurrentMarket _currentMarket;

        public Type BackingType = typeof(double);
        public List<SelectableNumericRange> Range;

        public FacetAverageRatingDefinition(ICurrentMarket currentMarket)
        {
            _currentMarket = currentMarket;
            RenderType = GetType().Name;
            Name = "Facet Test";
            DisplayName = "Facet Test Display Name";
            Range = new List<SelectableNumericRange>();
        }

        public TypeQueryBuilder<T> Filter<T>(TypeQueryBuilder<T> query, List<SelectableNumericRange> numericRanges)
        {
            if (numericRanges != null && numericRanges.Any())
            {
                //query = query.AddFilterForNumericRange(numericRanges, FieldName, BackingType);
                var ranges = numericRanges.Select(x => (x.From, x.To)).ToArray();
                query = query.Where(FieldName, new NumericFilterOperators().InRanges(ranges));
            }

            return query;
        }

        public override TypeQueryBuilder<T> Facet<T>(TypeQueryBuilder<T> query, IFacetOperator filter)
        {
            var range = Range.Where(x => x != null).ToList();
            if (!range.Any())
            {
                return query;
            }

            var convertedRangeList = range.Select(selectableNumericRange => selectableNumericRange.ToNumericRange())
                .ToList();
            //return query.RangeFacetFor(FieldName, typeof(double), filter, convertedRangeList);
            var facetFilter = new TermFacetFilter(FieldName, new NumericFacetFilterOperators().Ranges(convertedRangeList.Select(x => (x.From, x.To)).ToArray()));
            query.Facet(facetFilter);
            return query;
        }

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
                    Selected = selectedFacets != null && selectedFacets.Contains($"{x.Name}:{GetKey((from, to))}")
                };

            }).ToList();
        }

        private static string GetKey((double? From, double? To) result)
        {
            var from = result.From == null ? "MIN" : result.From.ToString();
            var to = result.To == null ? "MAX" : result.To.ToString();
            return from + "-" + to;
        }

        private string GetDisplayText((double? From, double? To) result)
        {
            var currency = _currentMarket.GetCurrentMarket().DefaultCurrency;

            var from = result.From == null
                ? new Money(0, currency).ToString()
                : new Money(Convert.ToDecimal(result.From.Value), currency).ToString();

            var to = result.To == null
                ? new Money(10000, currency).ToString()
                : new Money(Convert.ToDecimal(result.To.Value), currency).ToString();

            return from + "-" + to;
        }
    }
}