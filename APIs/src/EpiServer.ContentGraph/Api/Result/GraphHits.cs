using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class GraphHits<T>
    {
        [JsonIgnore]
        Dictionary<string, List<CGFacet>> rs = null;
        [JsonIgnore]
        public string Key { get; private set; }

        [JsonProperty("items")]
        public List<T> Hits { get; set; }
        public Dictionary<string, List<CGFacet>> Facets
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
                    rs = new Dictionary<string, List<CGFacet>>();
                    foreach (var key in keys)
                    {
                        rs.Add(key, RawFacets[key].ToObject<List<CGFacet>>());
                    }
                }
                return rs;
            }
        }
        [JsonProperty("facets")]
        private Dictionary<string, JArray> RawFacets { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
