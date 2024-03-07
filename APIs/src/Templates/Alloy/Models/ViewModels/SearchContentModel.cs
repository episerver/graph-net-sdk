using System;
using System.Collections.Generic;
using System.Web;
using AlloyTemplates.Models.Pages;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace AlloyTemplates.Models.ViewModels
{
    public class SearchContentModel : PageViewModel<SearchPage>
    {
        Injected<IHttpContextAccessor> HttpContextAccessor;
        public SearchContentModel(SearchPage currentPage) : base(currentPage)
        {
        }

        public bool SearchServiceDisabled { get; set; }
        public string SearchedQuery { get; set; }
        public int NumberOfHits { get; set; }
        public IEnumerable<SearchHit> Hits { get; set; }
        public IEnumerable<SearchFacet> Facets { get; set; }
        public string FacetName { get; set; }
        public int PageItems { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string GetPageUrl(int pageNumber)
        {
            return UriUtil.AddQueryString(GetPathAndQuery(), "p", pageNumber.ToString());
        }
        public string GetFacetFilter(string group)
        {
            return UriUtil.AddQueryString(RemoveQueryStringByKey(HttpContextAccessor.Service.HttpContext.Request.GetDisplayUrl(), "p"), "t",
                    HttpUtility.UrlEncode(group));
        }
        public int PagingPage
        {
            get
            {
                int pagingPage;
                if (!int.TryParse(HttpContextAccessor.Service.HttpContext.Request.Query["p"].ToString(), out pagingPage))
                {
                    pagingPage = 1;
                }

                return pagingPage;
            }
        }
        public string FacetTerm
        {
            get
            {
                return HttpContextAccessor.Service.HttpContext.Request.Query["t"].ToString();
            }
        }
        public int TotalPages
        {
            get
            {
                return 1 + (NumberOfHits - 1) / PageItems;
            }
        }
        private string GetPathAndQuery() => $"{HttpContextAccessor.Service.HttpContext.Request.Path}{HttpContextAccessor.Service.HttpContext.Request.QueryString.Value}";
        /// <summary>
        /// Removes specified query string from url
        /// </summary>
        /// <param name="url">Url from which to remove query string</param>
        /// <param name="key">Key of query string to remove</param>
        /// <returns>New url that excludes the specified query string</returns>
        private string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);
            newQueryString.Remove(key);
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }
        public class SearchHit
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Excerpt { get; set; }
        }
        public class SearchFacet
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }
    }
}
