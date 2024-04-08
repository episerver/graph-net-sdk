using EPiServer.ContentGraph.Helpers.Text;
namespace EPiServer.ContentGraph.Api.Querying
{
    public class HighLightOptions
    {
        public static HighLightOptions Create()
        {
            return new HighLightOptions();
        }
        string _query = string.Empty;
        public string Query => $"(highlight:{{{_query}}})";

        public HighLightOptions StartToken(string token)
        {
            _query += _query.IsNullOrEmpty() ? $"startToken:\"{token}\"" : $",startToken:\"{token}\"";
            return this;
        }
        public HighLightOptions EndToken(string token)
        {
            _query += _query.IsNullOrEmpty() ? $"endToken:\"{token}\"" : $",endToken:\"{token}\"";
            return this;
        }
        public HighLightOptions Enable(bool enable)
        {
            _query += _query.IsNullOrEmpty() ? $"enabled:{enable.ToString().ToLower()}" : $",enabled:{enable.ToString().ToLower()}";
            return this;
        }
    }
}
