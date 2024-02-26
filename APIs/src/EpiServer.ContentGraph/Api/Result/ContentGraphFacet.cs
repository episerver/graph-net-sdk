using Newtonsoft.Json;
namespace EPiServer.ContentGraph.Api.Result
{
    public class Facet
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
