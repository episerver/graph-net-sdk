using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphHits<T>
    {
        [JsonIgnore]
        Dictionary<string, List<ContentGraphFacet>> rs = null;

        [JsonProperty("items")]
        public List<T> Hits { get; set; }
        [JsonIgnore]
        public Dictionary<string, List<ContentGraphFacet>> Facets
        {
            get
            {
                if (rs != null)
                {
                    return rs;
                }
                if (RawFacets != null)
                {
                    var keys = RawFacets.Keys;
                    rs = new Dictionary<string, List<ContentGraphFacet>>();
                    foreach (var key in keys)
                    {
                        rs.Add(key, RawFacets[key].ToObject<List<ContentGraphFacet>>());
                    }
                }
                return rs;
            }
        }
        [JsonProperty("facets")]
        private Dictionary<string, JArray> RawFacets { get; set; }
        [JsonProperty("autocomplete")]
        private Dictionary<string, JArray> RawAutoComplete { get; set; }
        //public Dictionary<string, List<ContentGraphFacet>> AutoComplete
        //{
        //    get
        //    {
        //        if (rs != null)
        //        {
        //            return rs;
        //        }
        //        if (RawFacets != null)
        //        {
        //            var keys = RawFacets.Keys;
        //            rs = new Dictionary<string, List<ContentGraphFacet>>();
        //            foreach (var key in keys)
        //            {
        //                rs.Add(key, RawFacets[key].ToObject<List<ContentGraphFacet>>());
        //            }
        //        }
        //        return rs;
        //    }
        //}
        [JsonProperty("cursor")]
        public int Cursor { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
