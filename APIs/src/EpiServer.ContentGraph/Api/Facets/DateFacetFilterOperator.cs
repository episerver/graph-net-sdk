using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class DateFacetFilterOperator : IFacetFilterOperator
    {
        public DateFacetFilterOperator INSTANCE { get { return new DateFacetFilterOperator(); } }
        string _query = string.Empty;
        IEnumerable<FacetProjection> _projections;
        public string Query { get { return _query; } }
        public IEnumerable<FacetProjection> FacetProjections { get { return _projections; } }
        public DateFacetFilterOperator Unit(DateUnit dateUnit)
        {
            _query = _query.IsNullOrEmpty() ? $"unit:{dateUnit}" : $"{_query},unit:{dateUnit}";
            return this;
        }
        public DateFacetFilterOperator Value(int value)
        {
            _query = _query.IsNullOrEmpty() ? $"value:{value}" : $"{_query},value:{value}";
            return this;
        }
        public DateFacetFilterOperator Projection(params FacetProjection[] projections)
        {
            projections.ValidateNotNullArgument("projections");
            _projections = projections.Distinct().ToArray();
            return this;
        }
    }
}
