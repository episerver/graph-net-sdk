using Models = Optimizely.ContentGraph.DataModels;
namespace Foundation.Features.Locations.LocationItemPage
{
    public class LocationItemViewModel : ContentViewModel<LocationItemPage>
    {
        public LocationItemViewModel(LocationItemPage currentPage) : base(currentPage)
        {
            LocationNavigation = new LocationNavigationModel
            {
                CurrentLocation = currentPage
            };
        }

        public ImageData Image { get; set; }
        public LocationNavigationModel LocationNavigation { get; set; }
        public IEnumerable<LocationItemPage> SimilarLocations { get; set; }

        //public IEnumerable<StandardCategory> Tags { get; set; }
    }

    public class LocationNavigationModel
    {
        public LocationNavigationModel()
        {
            CloseBy = Enumerable.Empty<Models.LocationItemPage>();
            ContinentLocations = Enumerable.Empty<Models.LocationItemPage>();
        }

        public IEnumerable<Models.LocationItemPage> CloseBy { get; set; }
        public IEnumerable<Models.LocationItemPage> ContinentLocations { get; set; }
        public LocationItemPage CurrentLocation { get; set; }
    }


}