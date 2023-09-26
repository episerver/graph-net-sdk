using EPiServer.ContentGraph.Helpers;
using System.Text;

namespace EPiServer.ContentGraph.Api
{
    public class ContentGraphQuery
    {
        public string Filter { get; set; }
        //public long Limit { get; set; }
        //public long Skip { get; set; }
        //public string Locale { get; set; }
        //public string OrderBy { get; set; }
        public string WhereClause { get; set; }
        public string Autocomplete { get; set; }
        public string Cursor { get; set; }
        public string TypeName { get; set; }
        public string SelectItems { get; set; }
        public string Facets { get; set; }
        public string Total { get; set; }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .Append(TypeName)
                .Append(Filter)
                .Append("{")
                .Append(SelectItems)
                .Append(Facets)
                .Append(Autocomplete)
                .Append(Total)
                .Append(Cursor)
                .Append("}");
            return stringBuilder.ToString();
        }
    }
}
