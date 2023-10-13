using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class NumericFacetFilterOperator : IFacetFilterOperator
    {
        public NumericFacetFilterOperator INSTANCE { get { return new NumericFacetFilterOperator(); } }
        string _query = string.Empty;
        IEnumerable<FacetProjection> _projections;
        public string Query { get { return _query; } }
        public IEnumerable<FacetProjection> FacetProjections { get { return _projections; } }
        /// <summary>
        /// Ranges for facets. For ex: Ranges((1, 2), (9, 10))
        /// </summary>
        /// <param name="values"></param>
        /// <returns>NumericFacetFilterOperator</returns>
        public NumericFacetFilterOperator Ranges(params (long from, long to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => $"{{from:{x.from},to:{x.to}}}"));
            _query = _query.IsNullOrEmpty() ? $"ranges:[{combineRanges}]" : $",ranges:[{combineRanges}]";
            return this;
        }
        public NumericFacetFilterOperator Projection(params FacetProjection[] projections)
        {
            projections.ValidateNotNullArgument("projections");
            _projections = projections.Distinct().ToArray();
            return this;
        }
    }
}
