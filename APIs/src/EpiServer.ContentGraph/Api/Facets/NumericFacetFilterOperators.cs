using EPiServer.ContentGraph.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.ContentGraph.Api.Facets
{

    public class NumericFacetFilterOperators : IFacetOperator
    {
        string _query = string.Empty;
        IEnumerable<FacetProperty> _projections;
        public string FilterClause { get { return _query; } }
        public NumericFacetFilterOperators()
        {
            _projections = new List<FacetProperty> { FacetProperty.name, FacetProperty.count };
        }
        public IEnumerable<FacetProperty> FacetProjections { get { return _projections; } }
        /// <summary>
        /// Ranges for facets. For ex: Ranges((1, 2), (9, 10))
        /// </summary>
        /// <param name="values"></param>
        /// <returns>NumericFacetFilterOperator</returns>
        public NumericFacetFilterOperators Ranges(params (int? from, int? to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => GetRange(x)));
            _query = _query.IsNullOrEmpty() ? $"ranges:[{combineRanges}]" : $",ranges:[{combineRanges}]";
            return this;
        }
        public NumericFacetFilterOperators Ranges(params (long? from, long? to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => GetRange(x)));
            _query = _query.IsNullOrEmpty() ? $"ranges:[{combineRanges}]" : $",ranges:[{combineRanges}]";
            return this;
        }
        public NumericFacetFilterOperators Ranges(params (float? from, float? to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => GetRange(x)));
            _query = _query.IsNullOrEmpty() ? $"ranges:[{combineRanges}]" : $",ranges:[{combineRanges}]";
            return this;
        }
        public NumericFacetFilterOperators Ranges(params (double? from, double? to)[] values)
        {
            string combineRanges = string.Join(',', values.Select(x => GetRange(x)));
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
        private string GetRange((double? from, double? to) range)
        {
            string query = string.Empty;
            if (range.from.HasValue)
            {
                query = $"from:{range.from.Value.ToInvariantString()}";
            }
            if (range.to.HasValue)
            {
                query += query.IsNullOrEmpty() ? $"to:{range.to.Value.ToInvariantString()}" : $",to:{range.to.Value.ToInvariantString()}";
            }
            query = query.IsNullOrEmpty() ? query : $"{{{query}}}";
            return query;
        }
    }
}
