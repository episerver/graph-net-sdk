using Microsoft.AspNetCore.Http;

namespace EPiServer.ContentGraph.Helpers
{
    public static class HttpContextHelper
    {
        internal static IHttpContextAccessor _httpContextAccessor;

        public static IHttpContextAccessor Current
        {
            get { return _httpContextAccessor; }
            set { _httpContextAccessor = value; }
        }
    }
}
