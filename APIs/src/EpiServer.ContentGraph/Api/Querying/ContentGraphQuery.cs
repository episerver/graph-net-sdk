using EPiServer.ContentGraph.Helpers;
using System.Text;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class ContentGraphQuery
    {
        public string Filter { get; set; } = string.Empty;
        public string Limit { get; set; } = string.Empty;
        public string Skip { get; set; } = string.Empty;
        public string Ids { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string WhereClause { get; set; } = string.Empty;
        public string Autocomplete { get; set; } = string.Empty;
        public string Cursor { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public StringBuilder SelectItems { get; set; } = new StringBuilder();
        public string Facets { get; set; } = string.Empty;
        public string Total { get; set; } = string.Empty;
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(TypeName); //mandatory property
            if (!Filter.IsNullOrEmpty())
            {
                stringBuilder.Append(Filter);
            }
            stringBuilder.Append('{');
            stringBuilder.Append(SelectItems);
            if (!Facets.IsNullOrEmpty())
            {
                stringBuilder.Append(' ');
                stringBuilder.Append(Facets);
            }
            if (!Autocomplete.IsNullOrEmpty())
            {
                stringBuilder.Append(' ');
                stringBuilder.Append(Autocomplete);
            }
            if (!Total.IsNullOrEmpty())
            {
                stringBuilder.Append(' ');
                stringBuilder.Append(Total);
            }
            if (!Cursor.IsNullOrEmpty())
            {
                stringBuilder.Append(' ');
                stringBuilder.Append(Cursor);
            }
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }
    }
}
