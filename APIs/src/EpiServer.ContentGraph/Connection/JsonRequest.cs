using System.Net;
using System.Reflection;
using System.Text;
using EPiServer.ContentGraph.Helpers;
using EPiServer.ContentGraph.Tracing;

namespace EPiServer.ContentGraph.Connection
{
    public class JsonRequest : IJsonRequest, IDisposable
    {
        private static readonly Encoding utf8NoBom = new System.Text.UTF8Encoding(false/* no BOM */);
        private static readonly string userAgent = string.Format("EPiServer-Find-NET-API/{0}",
                                                                 typeof(JsonRequest).Assembly.GetName().Version);
        HttpWebRequest webRequest;
        HttpWebResponse webResponse;

        Guid traceGuid;

        public JsonRequest(string url, HttpVerbs method, Action<HttpWebRequest> webRequestAction)
        {
            traceGuid = Guid.NewGuid();

            Uri uri = new Uri(url);
            ForceCanonicalPathAndQuery(uri);
            webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            webRequest.Method = method.ToString().ToUpper();
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = userAgent;
            //webRequest.Headers.Add("Expect", "100-continue");
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (webRequestAction.IsNotNull())
            {
                webRequestAction(webRequest);
            }
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
            webRequest.Headers.Add(name, value);
        }

        public Uri RequestUri
        {
            get { return webRequest.RequestUri; }
        }

        public virtual Stream GetRequestStream(long contentLength)
        {
            webRequest.ContentLength = contentLength;
            return webRequest.GetRequestStream();
        }

        public virtual void WriteBody(string body)
        {
            Trace.Instance.Add(new TraceEvent(this, string.Format("Writing body to request: {0}.", body)));
            webRequest.ContentLength = utf8NoBom.GetByteCount(body);
            int bufferSize = Math.Min(body.Length, 4096 * 6);
            using (var requestBuffer = new StreamWriter(webRequest.GetRequestStream(), utf8NoBom, bufferSize, true))
            {
                requestBuffer.Write(body);
            }
        }

        public virtual string GetResponse()
        {
            Trace.Instance.Add(new TraceEvent(this, string.Format("Executing {0} request to {1}.", webRequest.Method, RequestUri)));
            string response;
            using (var responseStream = GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                response = streamReader.ReadToEnd();
            }
            //webResponse.Close();
            //webResponse = null;
            return response;
        }

        public virtual Dictionary<string, string> GetResponseHeaders()
        {

            var headers = webResponse.Headers;
            var headerDictionary = new Dictionary<string, string>();
            foreach(var header in headers.AllKeys)
            {
                foreach (var headerValue in headers.GetValues(header))
                {
                    headerDictionary.Add(header, headerValue);
                }
                
            }
            return headerDictionary;
        }

        public Stream GetResponseStream()
        {
            Trace.Instance.Add(new TraceEvent(this, string.Format("Executing {0} request to {1}.", webRequest.Method, RequestUri)));
            webResponse = (HttpWebResponse)webRequest.GetResponse();

            return webResponse.GetResponseStream();
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
            if (webResponse != null)
            {
                webResponse.Close();
                webResponse = null;
            }
        }
    }
}
