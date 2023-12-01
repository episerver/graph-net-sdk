using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{

    public class NumericFacetFilterOperator : IFacetOperator
    {
        string _query = string.Empty;
        IEnumerable<FacetProperty> _projections;
        public string FilterClause { get { return _query; } }
        public NumericFacetFilterOperator()
        {
            _projections = new List<FacetProperty> { FacetProperty.name, FacetProperty.count };
        }
        public IEnumerable<FacetProperty> FacetProjections { get { return _projections; } }
        /// <summary>
        /// Ranges for facets. For ex: Ranges((1, 2), (9, 10))
        /// </summary>
        /// <param name="values"></param>
        /// <returns>NumericFacetFilterOperator</returns>
        public NumericFacetFilterOperator Ranges(params (long? from, long? to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => GetRange(x)));
            _query = _query.IsNullOrEmpty() ? $"ranges:[{combineRanges}]" : $",ranges:[{combineRanges}]";
            return this;
        }
        public NumericFacetFilterOperator Ranges(params (float? from, float? to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => $"{{from:{x.from},to:{x.to}}}"));
            _query = _query.IsNullOrEmpty() ? $"ranges:[{combineRanges}]" : $",ranges:[{combineRanges}]";
            return this;
        }
        public IFacetOperator Projection(params FacetProperty[] projections)
        {
            projections.ValidateNotNullArgument("projections");
            _projections = projections.Distinct().ToArray();
            return this;
        }
        private string GetRange((long? from, long? to) range)
        {
            string query = string.Empty;
            if (range.from.HasValue)
            {
                query = $"from:{range.from.Value}";
            }
            if (range.to.HasValue)
            {
                query += query.IsNullOrEmpty() ? $"to:{range.to.Value}" : $",to:{range.to.Value}";
            }
            query = query.IsNullOrEmpty() ? query : $"{{{query}}}";
            return query;
        }
    }
}
