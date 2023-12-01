using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Facets
{
    public class DateFacetFilterOperator : IFacetOperator
    {
        string _query = string.Empty;
        IEnumerable<FacetProperty> _projections;
        public string FilterClause { get { return _query; } }
        public DateFacetFilterOperator()
        {
            _projections = new List<FacetProperty> { FacetProperty.name, FacetProperty.count };
        }
        public IEnumerable<FacetProperty> FacetProjections { get { return _projections; } }
        public DateFacetFilterOperator Unit(DateUnit dateUnit = DateUnit.DAY)
        {
            _query = _query.IsNullOrEmpty() ? $"unit:{dateUnit}" : $"{_query},unit:{dateUnit}";
            return this;
        }
        public DateFacetFilterOperator Value(int value = 1)
        {
            _query = _query.IsNullOrEmpty() ? $"value:{value}" : $"{_query},value:{value}";
            return this;
        }
        public IFacetOperator Projection(params FacetProperty[] projections)
        {
            projections.ValidateNotNullArgument("projections");
            _projections = projections.Distinct().ToArray();
            return this;
        }
    }
}
