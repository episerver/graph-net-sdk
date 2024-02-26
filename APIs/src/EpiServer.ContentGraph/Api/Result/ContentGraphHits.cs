using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphHits<T>
    {
        [JsonIgnore]
        Dictionary<string, IEnumerable<Facet>> facets = null;
        [JsonIgnore]
        Dictionary<string, IEnumerable<string>> autocompletes = null;
        [JsonProperty("items")]
        public IEnumerable<T> Hits { get; set; }
        [JsonIgnore]
        public Dictionary<string, IEnumerable<Facet>> Facets
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
                    facets = new Dictionary<string, IEnumerable<Facet>>();
                    foreach (var key in keys)
                    {
                        facets.Add(key, RawFacets[key].ToObject<IEnumerable<Facet>>());
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
