using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    internal class TermFilter: Filter
    {
        string _field = string.Empty;
        IFilterOperator _filterOperator;
        public override string FilterClause
        {
            get
            {
                return $"{ConvertNestedFieldToString.ConvertNestedFieldFilter(_field, _filterOperator)}";
            }
        }
        public TermFilter(string field, IFilterOperator filterOperator)
        {
            _field = field;
            _filterOperator = filterOperator;
        }
    }
}
