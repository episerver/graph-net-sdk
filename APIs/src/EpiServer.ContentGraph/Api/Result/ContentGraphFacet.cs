using Newtonsoft.Json;
namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphFacet
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
