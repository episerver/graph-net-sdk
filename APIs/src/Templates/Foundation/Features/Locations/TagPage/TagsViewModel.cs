namespace Foundation.Features.Locations.TagPage
{
    public class TagsViewModel : ContentViewModel<TagPage>
    {
        public TagsViewModel(TagPage currentPage) : base(currentPage)
        {
        }

        public string Continent { get; set; }

        public string[] AdditionalCategories { get; set; }

        public TagsCarouselViewModel Carousel { get; set; }

        public List<Optimizely.ContentGraph.DataModels.LocationItemPage> Locations { get; set; }
    }
}