using EPiServer.ContentGraph.Json;
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
                if (RawData.ContainsKey(typeName))
                {
                    return RawData[typeName].ToObject<ContentGraphHits<TResult>>(JsonSerializerHelper.CreateSerializerIgnoreNullValue());
                }
                else
                {
                    return RawData.Values.First().ToObject<ContentGraphHits<TResult>>(JsonSerializerHelper.CreateSerializerIgnoreNullValue());
                }
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
                return RawData[typeName].ToObject<ContentGraphHits<TOtherType>>(JsonSerializerHelper.CreateSerializerIgnoreNullValue());
            }
            catch (Exception e)
            {
                throw new Exception($"Can not cast data from type [{typeof(TOriginal).Name}] to [{typeof(TOtherType).Name}]", e);
            }
        }
        [JsonIgnore]
        public string[] DataTypes { get => RawData.Keys.ToArray(); }
        [JsonProperty("extensions")]
        public Extensions Extensions { get; set; }
        [JsonProperty("errors")]
        public ContentGraphError[] Errors { get; set; }
    }
    public class ContentGraphResult<T> : ContentGraphResult
    {
        [JsonIgnore]
        ContentGraphHits<T> _content = null;
        public ContentGraphHits<T> Content
        {
            get
            {
                if (_content != null)
                {
                    return _content;
                }

                _content = GetContent<T>();
                return _content;
            }
        }
    }
}
