using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Extensions;
//using EPiServer.Find;
//using EPiServer.Find.Cms;
//using EPiServer.Find.Framework;
using Foundation.Features.Media;
using Models = Optimizely.ContentGraph.DataModels;

namespace Foundation.Features.Locations.TagPage
{
    public class TagPageController : PageController<TagPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly GraphQueryBuilder _client;
        public TagPageController(IContentLoader contentLoader, GraphQueryBuilder graphQuery)
        {
            _contentLoader = contentLoader;
            _client = graphQuery;
        }

        public ActionResult Index(TagPage currentPage)
        {
            var model = new TagsViewModel(currentPage)
            {
                Continent = RouteData.Values["Continent"]?.ToString()
            };

            var addcat = RouteData.Values["Category"]?.ToString();
            if (addcat != null)
            {
                model.AdditionalCategories = addcat.Split(',');
            }

            //var query = SearchClient.Instance.Search<LocationItemPage.LocationItemPage>()
            //    .Filter(f => f.TagString().Match(currentPage.Name));
            var query = _client.ForType<Models.LocationItemPage>()
                   .Filter(_ => _.Name.Match(currentPage.Name))
                   .Fields(_=> _.Name, _=> _.PageImage.Url, _=> _.Url);
            if (model.AdditionalCategories != null)
            {
                //query = model.AdditionalCategories.Aggregate(query, (current, c) => current.Filter(f => f.TagString().Match(c)));
                query = model.AdditionalCategories.Aggregate(query, (current, c) => current.Filter(f => f.Name.Match(c)));
            }
            if (model.Continent != null)
            {
                //query = query.Filter(dp => dp.Continent.MatchCaseInsensitive(model.Continent));
                query = query.Filter(dp => dp.Continent.Eq(model.Continent));
            }
            //model.Locations = query.StaticallyCacheFor(new System.TimeSpan(0, 1, 0)).GetContentResult().ToList();
            model.Locations = query.GetResultAsync<Models.LocationItemPage>().Result.Content.Hits.ToList();
            //Add theme images from results
            var carousel = new TagsCarouselViewModel
            {
                Items = new List<TagsCarouselItem>()
            };
            foreach (var location in model.Locations)
            {
                if (location.Image != null)
                {
                    carousel.Items.Add(new TagsCarouselItem
                    {
                        Image = location.Image,
                        Heading = location.Name,
                        Description = location.MainIntro,
                        ItemURL = location.ContentLink
                    });
                }
            }
            if (carousel.Items.All(item => item.Image == null) || currentPage.Images != null)
            {
                if (currentPage.Images != null && currentPage.Images.FilteredItems != null)
                {
                    foreach (var image in currentPage.Images.FilteredItems.Select(ci => ci.ContentLink))
                    {
                        var title = _contentLoader.Get<ImageMediaData>(image).Title;
                        carousel.Items.Add(new TagsCarouselItem { Image = new Models.ContentModelReference { Url = image.GetUri().AbsoluteUri, WorkId = image.WorkID, Id = image.ID}, Heading = title });
                    }
                }
            }
            model.Carousel = carousel;

            return View(model);
        }
    }
}