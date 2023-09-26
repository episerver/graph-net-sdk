using EPiServer.ContentGraph.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class GraphResult<T>
    {
        [JsonIgnore]
        Dictionary<string, GraphHits<T>> rs = null;

        [JsonProperty("data")]
        private Dictionary<string, JObject> RawData { get; set; }
        public Dictionary<string, GraphHits<T>> Content
        {
            get
            {
                if (rs != null)
                {
                    return rs;
                }
                if (RawData != null)
                {
                    rs = new Dictionary<string, GraphHits<T>>();
                    DataTypes = RawData.Keys.ToStringArray();
                    foreach (var key in DataTypes)
                    {
                        rs.Add(key, RawData[key].ToObject<GraphHits<T>>());
                    }
                }
                return rs;
            }
        }
        [JsonIgnore]
        public string[] DataTypes { get; private set; }
        [JsonProperty("extensions")]
        public object Extension { get; set; }
        [JsonProperty("errors")]
        public object Errors { get; set; }
    }
}
