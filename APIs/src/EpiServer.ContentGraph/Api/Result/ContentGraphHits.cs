using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphHits<T>
    {
        [JsonIgnore]
        Dictionary<string, List<ContentGraphFacet>> facets = null;
        [JsonIgnore]
        Dictionary<string, List<string>> autocompletes = null;
        [JsonProperty("items")]
        public List<T> Hits { get; set; }
        [JsonIgnore]
        public Dictionary<string, List<ContentGraphFacet>> Facets
        {
            get
            {
                if (facets != null)
                {
                    return facets;
                }
                if (RawFacets != null)
                {
                    var keys = RawFacets.Keys;
                    facets = new Dictionary<string, List<ContentGraphFacet>>();
                    foreach (var key in keys)
                    {
                        facets.Add(key, RawFacets[key].ToObject<List<ContentGraphFacet>>());
                    }
                }
                return facets;
            }
        }
        [JsonIgnore]
        public Dictionary<string, List<string>> AutoComplete
        {
            get
            {
                if (autocompletes != null)
                {
                    return autocompletes;
                }
                if (RawAutoComplete != null)
                {
                    var keys = RawAutoComplete.Keys;
                    autocompletes = new Dictionary<string, List<string>>();
                    foreach (var key in keys)
                    {
                        autocompletes.Add(key, RawAutoComplete[key].ToObject<List<string>>());
                    }
                }
                return autocompletes;
            }
        }
        [JsonProperty("facets")]
        private Dictionary<string, JArray> RawFacets { get; set; }
        [JsonProperty("autocomplete")]
        private Dictionary<string, JArray> RawAutoComplete { get; set; }
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
