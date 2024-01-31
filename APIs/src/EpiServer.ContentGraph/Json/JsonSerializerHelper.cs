using Newtonsoft.Json;
namespace EPiServer.ContentGraph.Json
{
    public class JsonSerializerHelper
    {
        private static JsonSerializer _jsonSerializer = null;
        public static JsonSerializer CreateSerializerIgnoreNullValue()
        {
            if (_jsonSerializer != null)
            {
                return _jsonSerializer;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            _jsonSerializer = JsonSerializer.CreateDefault(settings);
            return _jsonSerializer;
        }
    }
}
