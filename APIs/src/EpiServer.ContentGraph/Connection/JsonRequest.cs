using System.Reflection;
using System.Text;
using EPiServer.ContentGraph.Tracing;

namespace EPiServer.ContentGraph.Connection
{
    public class JsonRequest : IJsonRequest, IDisposable
    {
        private static readonly Encoding utf8NoBom = new UTF8Encoding(false/* no BOM */);
        private static readonly string userAgent = string.Format("OptiGraph-NET-API/{0}",
                                                                 typeof(JsonRequest).Assembly.GetName().Version);
        private readonly HttpClient _httpClient;
        HttpRequestMessage request;

        Guid traceGuid;

        public JsonRequest(string url, HttpMethod method, HttpClient httpClient)
        {
            traceGuid = Guid.NewGuid();

            Uri uri = new Uri(url);
            ForceCanonicalPathAndQuery(uri);
            _httpClient = httpClient;
            request = new HttpRequestMessage(method, uri);
            request.Method = method;
            request.Headers.UserAgent.ParseAdd(userAgent);
        }

        private void ForceCanonicalPathAndQuery(Uri uri)
        {
            string paq = uri.PathAndQuery; // need to access PathAndQuery
            FieldInfo flagsFieldInfo = typeof(Uri).GetField("_flags", BindingFlags.Instance | BindingFlags.NonPublic);
            ulong flags = (ulong)flagsFieldInfo.GetValue(uri);
            flags &= ~((ulong)0x30); // Flags.PathNotCanonical|Flags.QueryNotCanonical
            flagsFieldInfo.SetValue(uri, flags);
        }

        public void AddRequestHeader(string name, string value)
        {
            request.Headers.Add(name, value);
        }

        public Uri RequestUri
        {
            get { return request.RequestUri; }
        }

        public async Task<Stream> GetResponseStream(string body)
        {
            
            Trace.Instance.Add(new TraceEvent(this, string.Format("Executing {0} request to {1}.", request.Method, RequestUri)));

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStream();
        }

        public Guid TraceId
        {
            get { return traceGuid; }
        }

        public Encoding Encoding
        {
            get { return utf8NoBom; }
        }

        public void Dispose()
        {
        }
    }
}
