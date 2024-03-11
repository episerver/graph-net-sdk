//using EPiServer.Find;
//using EPiServer.Find.Cms;
//using EPiServer.Find.Framework;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Extensions;
using Foundation.Extensions;
using Models = Optimizely.ContentGraph.DataModels;

namespace Foundation.Features.Locations.LocationItemPage
{
    public class LocationItemPageController : PageController<LocationItemPage>
    {
        private readonly IContentRepository _contentRepository;
        private readonly GraphQueryBuilder _client;
        public LocationItemPageController(IContentRepository contentRepository, GraphQueryBuilder client)
        {
            _contentRepository = contentRepository;
            _client = client;
        }

        public ActionResult Index(LocationItemPage currentPage)
        {
            var model = new LocationItemViewModel(currentPage);
            if (currentPage.Image != null)
            {
                model.Image = _contentRepository.Get<ImageData>(currentPage.Image);
            }

            model.LocationNavigation.ContinentLocations = _client
                .ForType<Models.LocationItemPage>()
                .Fields(x => x.Url, _ => _.Name, _ => _.Image.Id, _ => _.ContentLink.WorkId, _ => _.ContentLink.Id)
                .Total()
                .Filter(x => x.Continent.Match(currentPage.Continent))
                //.PublishedInCurrentLanguage()
                .OrderBy(x => x.Name)
                .FilterForVisitor()
                .Take(100)
                //.StaticallyCacheFor(new System.TimeSpan(0, 10, 0))
                .GetResultAsync<Models.LocationItemPage>()
                .Result
                .Content
                .Hits;

            var uri = currentPage.ContentLink.GetUri().AbsoluteUri;

            model.LocationNavigation.CloseBy = _client
                .ForType<Models.LocationItemPage>()
                .Fields(x => x.Url, _ => _.Name, _ => _.Image.Id, _ => _.ContentLink.WorkId, _ => _.ContentLink.Id)
                .Total()
                .Filter(x => x.Continent.Match(currentPage.Continent)
                             & !x.Url.Eq(uri))
                //.PublishedInCurrentLanguage()
                .FilterForVisitor()
                //.OrderBy(x => x.Coordinates)
                //.DistanceFrom(currentPage.Coordinates)
                .OrderBy(x => x.Longitude)
                .OrderBy(x => x.Latitude)
                .Take(5)
                //.StaticallyCacheFor(new System.TimeSpan(0, 10, 0))
                .GetResultAsync<Models.LocationItemPage>()
                .Result
                .Content
                .Hits;

            //if (currentPage.Categories != null)
            //{
            //    model.Tags = currentPage.Categories.Select(x => _contentRepository.Get<StandardCategory>(x));
            //}

            var editingHints = ViewData.GetEditHints<LocationItemViewModel, LocationItemPage>();
            editingHints.AddFullRefreshFor(p => p.Image);
            //editingHints.AddFullRefreshFor(p => p.Categories);

            return View(model);
        }

        private IEnumerable<Models.LocationItemPage> GetRelatedLocations(Models.LocationItemPage currentPage)
        {
            var distanceFilter = BooleanFilter.AndFilter<Models.LocationItemPage>();
            distanceFilter.And(x => x.Longitude, new NumericFilterOperators().Lte(currentPage.Longitude + 10).Boost(3));
            distanceFilter.And(x => x.Latitude, new NumericFilterOperators().Lte(currentPage.Latitude + 10).Boost(3));

            var query = _client
                .ForType<Models.LocationItemPage>()
                //.MoreLike(SearchTextFly(currentPage)) // sot support morelikethis query
                //.BoostMatching(x =>
                //    x.Country.Match(currentPage.Country ?? ""), 2)
                //.BoostMatching(x =>
                //    x.Continent.Match(currentPage.Continent ?? ""), 1.5)
                //.BoostMatching(x =>
                //    x.Coordinates
                //        .WithinDistanceFrom(currentPage.Coordinates ?? new GeoLocation(0, 0),
                //            1000.Kilometers()), 2.5)
                .Filter(x => x.Country, BoostMatching(currentPage.Country ?? "", 2))
                .Filter(x => x.Continent, BoostMatching(currentPage.Continent ?? "", 1))
                .Filter(distanceFilter)
                ;

            //query = currentPage.Category.Aggregate(query,
            //    (current, category) =>
            //        current.BoostMatching(x => x.InCategory(category), 1.5)); 
            query = currentPage.Category.Aggregate(query,
                (current, category) => current.Filter(x => x.Category.InCategory(category, 1)));

            return query
                .Filter(x => x.ParentLink.Url.Eq(currentPage.Url))
                //.PublishedInCurrentLanguage()
                .FilterForVisitor()
                .Take(3)
                .GetResultAsync<Models.LocationItemPage>()
                .Result
                .Content
                .Hits
                ;
        }
        private StringFilterOperators BoostMatching(string matchValue, int boostValue)
        {
            return new StringFilterOperators().Match(matchValue).Boost(boostValue);
        }
        public virtual string SearchTextFly(Models.LocationItemPage currentPage)
        {
            return "";
        }
    }
}