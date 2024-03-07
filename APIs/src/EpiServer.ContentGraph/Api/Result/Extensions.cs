using Newtonsoft.Json;

namespace EPiServer.ContentGraph.Api.Result
{
    public class Extensions
    {
        [JsonProperty("correlationId")]
        public string CorrelationID { get; set; }
        [JsonProperty("cost")]
        public int Cost { get; set; }
        [JsonProperty("costSummary")]
        public string[] CostSummary { get; set; }
    }
}
