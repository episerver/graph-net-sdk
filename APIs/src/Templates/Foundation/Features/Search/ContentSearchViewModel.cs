using EPiServer.Find.UnifiedSearch;

namespace Foundation.Features.Search
{
    //public class ContentSearchViewModel
    //{
    //    public UnifiedSearchResults Hits { get; set; }
    //    public FilterOptionViewModel FilterOption { get; set; }

    //    public string SectionFilter
    //    {
    //        get
    //        {
    //            var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
    //            if (accessor.HttpContext == null)
    //            {
    //                return string.Empty;
    //            }

    //            return accessor.HttpContext.Request.Query["t"].ToString();
    //        }
    //    }

    //    public string GetSectionGroupUrl(string groupName)
    //    {
    //        var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
    //        if (accessor.HttpContext == null)
    //        {
    //            return string.Empty;
    //        }
    //        string url = UriUtil.AddQueryString(accessor.HttpContext.Request.GetDisplayUrl(), "t", WebUtility.UrlEncode(groupName));
    //        url = UriUtil.AddQueryString(url, "p", "1");
    //        return url;
    //    }
    //}
    public class ContentGraphSearchViewModel
    {
        public IEnumerable<SearchHit> Hits { get; set; }
        public FilterOptionViewModel FilterOption { get; set; }
        public IEnumerable<SearchFacet> Facets { get; set; }
        public string SectionFilter
        {
            get
            {
                var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
                if (accessor.HttpContext == null)
                {
                    return string.Empty;
                }

                return accessor.HttpContext.Request.Query["t"].ToString();
            }
        }

        public string GetSectionGroupUrl(string groupName)
        {
            var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
            if (accessor.HttpContext == null)
            {
                return string.Empty;
            }
            string url = UriUtil.AddQueryString(accessor.HttpContext.Request.GetDisplayUrl(), "t", WebUtility.UrlEncode(groupName));
            url = UriUtil.AddQueryString(url, "p", "1");
            return url;
        }

        public class SearchHit
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Excerpt { get; set; }
            public string ImageUri { get; set; }
        }
        public class SearchFacet
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }
    }
}