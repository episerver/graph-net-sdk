using EPiServer.ContentGraph.Helpers;
using System.Collections.Generic;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class ModifiedFilter : IFilter
    {
        string _query = string.Empty;

        public string FilterClause => $"_modified:{{{_query}}}";

        public IList<IFilter> Filters { get; set; }

        public ModifiedFilter Modified(DateFilterOperators filterOperator)
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
