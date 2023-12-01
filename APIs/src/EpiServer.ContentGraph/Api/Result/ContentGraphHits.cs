using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphHits<T>
    {
        [JsonIgnore]
        Dictionary<string, IEnumerable<ContentGraphFacet>> facets = null;
        [JsonIgnore]
        Dictionary<string, IEnumerable<string>> autocompletes = null;
        [JsonProperty("items")]
        public IEnumerable<T> Hits { get; set; }
        [JsonIgnore]
        public Dictionary<string, IEnumerable<ContentGraphFacet>> Facets
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
                    facets = new Dictionary<string, IEnumerable<ContentGraphFacet>>();
                    foreach (var key in keys)
                    {
                        facets.Add(key, RawFacets[key].ToObject<IEnumerable<ContentGraphFacet>>());
                    }
                }
                return facets;
            }
        }
        [JsonIgnore]
        public Dictionary<string, IEnumerable<string>> AutoComplete
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
                    autocompletes = new Dictionary<string, IEnumerable<string>>();
                    foreach (var key in keys)
                    {
                        autocompletes.Add(key, RawAutoComplete[key].ToObject<IEnumerable<string>>());
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
