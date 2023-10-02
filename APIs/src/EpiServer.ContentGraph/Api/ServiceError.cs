using Newtonsoft.Json;

namespace EPiServer.ContentGraph.Api
{
    [Serializable]
    public class ServiceError
    {
        [JsonProperty("errors")]
        public string Errors { get; set; }
        [JsonProperty("extensions")]
        public string Extensions { get; set; }
    }
}
