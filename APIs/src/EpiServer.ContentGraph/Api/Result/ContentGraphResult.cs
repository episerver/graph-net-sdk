using EPiServer.ContentGraph.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphResult<T>
    {
        [JsonIgnore]
        Dictionary<string, ContentGraphHits<T>> rs = null;
        [JsonIgnore]
        public int StatusCode { get; set; } = 200;
        [JsonProperty("data")]
        private Dictionary<string, JObject> RawData { get; set; }
        public Dictionary<string, ContentGraphHits<T>> Content
        {
            get
            {
                if (rs != null)
                {
                    return rs;
                }
                if (RawData != null)
                {
                    rs = new Dictionary<string, ContentGraphHits<T>>();
                    DataTypes = RawData.Keys.ToStringArray();
                    foreach (var key in DataTypes)
                    {
                        rs.Add(key, RawData[key].ToObject<ContentGraphHits<T>>());
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
