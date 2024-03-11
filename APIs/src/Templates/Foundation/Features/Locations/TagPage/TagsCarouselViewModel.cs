using Optimizely.ContentGraph.DataModels;

namespace Foundation.Features.Locations.TagPage
{
    public class TagsCarouselViewModel
    {
        public List<TagsCarouselItem> Items { get; set; }
    }

    public class TagsCarouselItem
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public ContentModelReference Image { get; set; }
        public ContentModelReference ItemURL { get; set; }
    }
}