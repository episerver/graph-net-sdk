using System.Linq;
using AlloyTemplates.Controllers;
using AlloyTemplates.Models.Pages;
using AlloyTemplates.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EPiServer.ContentGraph.Api.Querying;
using ProxyModels = EPiServer.ContentGraph.DataModels;
using EPiServer.ContentGraph.Extensions;
using System.Collections.Generic;
using EPiServer.ContentGraph.Api;

namespace AlloyMvcTemplates.Controllers
{
    public class SearchPageController : PageControllerBase<SearchPage>
    {
        private GraphQueryBuilder _client;
        public SearchPageController(GraphQueryBuilder client)
        {
            _client = client;
        }

        public ViewResult Index(SearchPage currentPage, string q, string t, string p = "1")
        {
            var query = _client
                .OperationName("Alloy_Sample_Query")
                    .ForType<ProxyModels.Content>()
                    .Skip((int.Parse(p) -1) * 10)
                    .Limit(10)
                    .Fields(x=>x.Name, x=> x.Url)
                    .Total()
                        .AsType<ProxyModels.ArticlePage>(x=>x.MetaDescription, x=> x.MetaTitle)
                    .Search(q)
                    .Facet(x=>x.ContentType.FacetFilters(t))
                    .ToQuery()
                .BuildQueries();

            var content = query.GetResultAsync().Result.GetContent<ProxyModels.Content,ProxyModels.ArticlePage>();
            var hits = content.Hits;
            var facets = content.Facets["ContentType"];
            var model = new SearchContentModel(currentPage)
            {
                Hits = hits?.Select(x => new SearchContentModel.SearchHit
                {
                    Url = x.Url,
                    Excerpt = x.MetaDescription,
                    Title = x.Name
                })
                ,
                Facets = facets?.Select(x=> new SearchContentModel.SearchFacet { 
                    Name = x.Name,
                    Count = x.Count
                }),
                NumberOfHits = content.Total,
                SearchServiceDisabled = false,
                SearchedQuery = q,
                FacetName = content.Facets.Keys.First()
            };

            return View(model);
        }
    }
}
