using System.Linq;
using AlloyTemplates.Controllers;
using AlloyTemplates.Models.Pages;
using AlloyTemplates.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EPiServer.Shell.Search;
using EPiServer.ServiceLocation;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.Core;
using EPiServer.ContentGraph.Configuration;
using Microsoft.Extensions.Options;
using AlloyMvcTemplates.Models;
using EPiServer.ContentGraph.Extensions;
using EPiServer.ContentGraph.Api.Filters;
using System;

namespace AlloyMvcTemplates.Controllers
{
    public class Test
    {
        public string MetaTitle { get; set; }
    }
    public class SearchPageController : PageControllerBase<SearchPage>
    {
        private GraphQueryBuilder _client;
        public SearchPageController(GraphQueryBuilder client)
        {
            _client = client;
        }

        public ViewResult Index(SearchPage currentPage, string q)
        {
            //var query = _client.OperationName("AlloyQuery")
            //        .ForType<Models.ArticlePage>()
            //        .Field(x => x.StopPublish)
            //        .Fields(x => x.MetaTitle, x => x.MetaDescription, x => x.Language.Name, x => x.ContentLink.Url)
            //        .Search(q)
            //        .ToQuery()
            //        .BuildQueries();

            //var query = _client.OperationName("Fragment")
            //    .ForType<Content>()
            //    .Fields(x => x.Name)
            //        .ForSubType<Models.ArticlePage>(x => x.MetaTitle, x => x.MetaDescription)
            //        .ForSubType<Models.NewsPage>(x => x.MetaTitle, x => x.MetaDescription)
            //        .ForSubType<Models.ContactPage>(x => x.MetaTitle, x => x.MetaDescription)
            //    .Search(q)
            //    .ToQuery()
            //    .BuildQueries();
            //var rs = query.GetResult();
            //var hits = rs.GetCastingContent<Models.Content, Models.ArticlePage>().Hits;

            var query = _client.OperationName("Multiple_Types")
                .ForType<Models.ArticlePage>()
                    .Field("__typename")
                    .Fields(x => x.Name, x => x.MetaTitle, x => x.MetaDescription)
                    //.Where(x=> x.Name.Match(q))
                    .Search(q)
                    .FilterForVisitor()
                    .ToQuery()
                .ForType<Models.NewsPage>()
                    .Fields(x => x.Name, x => x.MetaDescription)
                    .Search(q)
                    .ToQuery()
                .BuildQueries();

            var rs = query.GetResult();
            var hits = rs.GetContent<Models.ArticlePage>().Hits;
            var hits2 = rs.GetContent<Models.NewsPage>().Hits;

            var model = new SearchContentModel(currentPage)
            {
                Hits = hits.Select(x => new SearchContentModel.SearchHit
                {
                    Url = x.Url,
                    Excerpt = x.MetaDescription,
                    Title = x.Name
                })
                .Concat(hits2.Select(x => new SearchContentModel.SearchHit
                {
                    Url = x.Url,
                    Excerpt = x.MetaDescription,
                    Title = x.Name
                }))
                ,
                NumberOfHits = hits.Count,
                SearchServiceDisabled = false,
                SearchedQuery = q
            };

            return View(model);
        }
    }
}
