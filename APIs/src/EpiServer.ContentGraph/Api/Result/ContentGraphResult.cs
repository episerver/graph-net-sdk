using EPiServer.ContentGraph.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPiServer.ContentGraph.Api.Result
{
    public class ContentGraphResult
    {
        [JsonProperty("data")]
        private Dictionary<string, JObject> RawData { get; set; }
        public ContentGraphHits<TResult> GetContent<TResult>()
        {
            try
            {
                string typeName = typeof(TResult).Name;
                return RawData[typeName].ToObject<ContentGraphHits<TResult>>();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not convert response to type [{typeof(TResult).Name}]", e);
            }
        }
        public ContentGraphHits<TOtherType> GetContent<TOriginal,TOtherType>()
        {
            try
            {
                string typeName = typeof(TOriginal).Name;
                return RawData[typeName].ToObject<ContentGraphHits<TOtherType>>();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not cast data from type [{typeof(TOriginal).Name}] to [{typeof(TOtherType).Name}]", e);
            }
        }
        [JsonIgnore]
        public string[] DataTypes { get => RawData.Keys.ToArray(); }
        [JsonProperty("extensions")]
        public object Extension { get; set; }
        [JsonProperty("errors")]
        public ContentGraphError[] Errors { get; set; }
    }
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
                    foreach (var key in DataTypes)
                    {
                        rs.Add(key, RawData[key].ToObject<ContentGraphHits<T>>());
                    }
                }
                return rs;
            }
        }

        public ContentGraphHits<TResult> GetContent<TResult>()
        {
            try
            {
                string typeName = typeof(TResult).Name;
                JObject keyValues;
                if (RawData.TryGetValue(typeName, out keyValues))
                {
                    return keyValues.ToObject<ContentGraphHits<TResult>>();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        [JsonIgnore]
        public string[] DataTypes { get => RawData.Keys.ToStringArray(); }
        [JsonProperty("extensions")]
        public object Extension { get; set; }
        [JsonProperty("errors")]
         public ContentGraphError[] Errors { get; set; }
    }
}
