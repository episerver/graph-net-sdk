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
                        GetJArray(RawFacets[key], key, facets);
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
                        GetJArray(RawAutoComplete[key], key, autocompletes);
                    }
                }
                return autocompletes;
            }
        }
        [JsonProperty("facets")]
        private Dictionary<string, object> RawFacets { get; set; }
        [JsonProperty("autocomplete")]
        private Dictionary<string, object> RawAutoComplete { get; set; }
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        private void GetJArray<TReturn>(object jObject, string key, Dictionary<string, IEnumerable<TReturn>> keyValues)
        {
            switch (jObject.GetType().Name)
            {
                case "JObject":
                    foreach (var token in ((JObject)jObject).Values())
                    {
                        string tempKey = key;
                        if (token.GetType() == typeof(JArray))
                        {
                            tempKey = $"{tempKey}.{token.Path}";
                            keyValues.Add(tempKey, ((JArray)token).ToObject<IEnumerable<TReturn>>());
                        }
                        else
                        {
                            GetJArray(token, tempKey, keyValues);
                        }
                    }
                    break;
                case "JArray":
                    keyValues.Add(key, ((JArray)jObject).ToObject<IEnumerable<TReturn>>());
                    break;
                case "JProperty":
                    break;
                default:
                    break;
            }
        }
    }
}
