using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class ModifiedFilter : IFilter
    {
        string _query = string.Empty;

        public string FilterClause => $"_modified:{{{_query}}}";

        public List<IFilter> Filters { get; set; }

        public ModifiedFilter Modified(IFilterOperator filterOperator)
        {
            if (_query.IsNullOrEmpty())
            {
                _query = $"{filterOperator.Query}";
            }
            else
            {
                _query += $",{filterOperator.Query}";
            }
            return this;
        }
    }
}
