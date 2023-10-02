using EPiServer.ContentGraph.Helpers;
using System.Text;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class ContentGraphQuery
    {
        public string Filter { get; set; }
        //public long Limit { get; set; }
        //public long Skip { get; set; }
        //public string Locale { get; set; }
        public string OrderBy { get; set; }
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
            stringBuilder.Append(TypeName); //mandatory property
            if (!Filter.IsNullOrEmpty())
            {
                stringBuilder.Append(Filter);
            }
            stringBuilder.Append('{');
            stringBuilder.Append(SelectItems);//mandatory property
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
