using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

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
                return RawData[typeName].ToObject<ContentGraphHits<TResult>>(JsonSerializerHelper.CreateSerializerIgnoreNullValue());
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
    public class ContentGraphResult<T>
    {
        [JsonIgnore]
        ContentGraphHits<T> rs = null;
        [JsonProperty("data")]
        private Dictionary<string, JObject> RawData { get; set; }
        public ContentGraphHits<T> Content
        {
            get
            {
                if (rs != null)
                {
                    return rs;
                }

                return GetContent<T>();
            }
        }
        private ContentGraphHits<TResult> GetContent<TResult>()
        {
            try
            {
                string typeName = typeof(TResult).Name;
                JObject keyValues;
                if (RawData != null && RawData.TryGetValue(typeName, out keyValues) && keyValues != null)
                {
                    return keyValues.ToObject<ContentGraphHits<TResult>>(JsonSerializerHelper.CreateSerializerIgnoreNullValue());
                }
                return null;
            }
            catch (Exception e)
            {
               throw new Exception($"Can not convert response to type [{typeof(TResult).Name}]", e);
            }
        }
        [JsonIgnore]
        public string[] DataTypes { get => RawData.Keys.ToStringArray(); }
        [JsonProperty("extensions")]
        public Extensions Extensions { get; set; }
        [JsonProperty("errors")]
         public ContentGraphError[] Errors { get; set; }
    }
}
