using Newtonsoft.Json;

namespace EPiServer.ContentGraph.Api
{
    [Serializable]
    public class ContentGraphError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("locations")]
        public TraceLocation[] Locations { get; set; }
    }
    [Serializable]
    public class TraceLocation {
        [JsonProperty("line")]
        public int Line { get; set; }
        [JsonProperty("column")]
        public int Column { get; set; }
    }
}
