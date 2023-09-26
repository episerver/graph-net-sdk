using System;
using System.Net;

namespace EPiServer.ContentGraph.Connection
{
    public class JsonRequestFactory : IJsonRequestFactory
    {
        private int? defaultRequestTimeout;

        public JsonRequestFactory()
            : this(null) {}

        public JsonRequestFactory(int? defaultRequestTimeout)
        {
            this.defaultRequestTimeout = defaultRequestTimeout;
        }

        public virtual IJsonRequest CreateRequest(string url, HttpVerbs method, int? explicitRequestTimeout)
        {
            Action<HttpWebRequest> requestAction = null;
            if (explicitRequestTimeout.HasValue)
            {
                requestAction = request => request.Timeout = explicitRequestTimeout.Value;
            }
            else if (defaultRequestTimeout.HasValue)
            {
                requestAction = request => request.Timeout = defaultRequestTimeout.Value;
            }
            return new JsonRequest(url, method, requestAction);
        }
    }
}