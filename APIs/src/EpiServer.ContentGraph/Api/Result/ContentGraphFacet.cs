using Newtonsoft.Json;
namespace EPiServer.ContentGraph.Api.Result
{
    public class CGFacet
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
