namespace EPiServer.ContentGraph.Extensions
{
    public static class QueryExtension
    {
        public static string Alias(this string field, string alias)
        {
            string actualField = string.Join('.',field.Split(".").Skip(1));
            return alias + ":" + actualField;
        }
    }
}
