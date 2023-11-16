using System.Linq;
using AlloyTemplates.Controllers;
using AlloyTemplates.Models.Pages;
using AlloyTemplates.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EPiServer.ContentGraph.Api.Querying;
using ProxyModels = EPiServer.ContentGraph.DataModels;
using EPiServer.ContentGraph.Extensions;

namespace AlloyMvcTemplates.Controllers
{
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
            //        .ForType<Models.Content>()
            //        .Fields(x => x.Language.Name, x => x.Url, x=> x.Name)
            //        .Search(q)
            //        .ToQuery()
            //        .BuildQueries();

            var query = _client.OperationName("Fragment")
                .ForType<ProxyModels.Content>()
                .Fields(x => x.Name, x=> x.Url)
                    .ForSubType<ProxyModels.ArticlePage>(x => x.MetaTitle, x => x.MetaDescription)
                    .ForSubType<ProxyModels.NewsPage>(x => x.MetaTitle, x => x.MetaDescription)
                    .ForSubType<ProxyModels.ContactPage>(x => x.MetaTitle, x => x.MetaDescription)
                .Search(q)
                .Facet(x=>x.Status, new EPiServer.ContentGraph.Api.Facets.StringFacetFilterOperator().Limit(1))
                .Facet(x=>x.Name.FacetLimit(10))
                .ToQuery()
                .BuildQueries();

            //var rs = query.GetResult();
            //var hits = rs.GetCastingContent<Models.Content, Models.ArticlePage>().Hits;

            //var query = _client.OperationName("Multiple_Types")
            //    .ForType<ProxyModels.ArticlePage>()
            //        .Fields(x => x.Name, x => x.MetaTitle, x => x.MetaDescription)
            //        .Search(q)
            //        .FilterForVisitor()
            //        .ToQuery()
            //    .ForType<ProxyModels.ContactPage>()
            //        .Fields(x => x.Name, x => x.MetaDescription)
            //        .Search(q)
            //        .ToQuery()
            //    .BuildQueries();

            var rs = query.GetResult().Result;
            var hits = rs.GetContent< ProxyModels.ArticlePage>().Hits;
            var hits2 = rs.GetContent<ProxyModels.ContactPage>().Hits;

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
