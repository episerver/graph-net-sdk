using Optimizely.ContentGraph.DataModels;

namespace Foundation.Features.Category
{
    public class CategorySearchViewModel : StandardCategoryViewModel
    {
        public CategorySearchViewModel() { }
        public CategorySearchViewModel(StandardCategory category) : base(category) { }

        public CategorySearchResults SearchResults { get; set; }
    }

    public class CategorySearchResults
    {
        public CategorySearchResults()
        {
            RelatedPages = new List<Optimizely.ContentGraph.DataModels.FoundationPageData>();
            Pagination = new Pagination();
        }

        public IEnumerable<Optimizely.ContentGraph.DataModels.FoundationPageData> RelatedPages { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public Pagination()
        {
            Page = 1;
            PageSize = 15;
            Categories = new List<ContentModelReference>();
            Sort = CategorySorting.PublishedDate.ToString();
            SortDirection = "desc";
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalMatching { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
    }

    public enum CategorySorting
    {
        PublishedDate,
        Name,
    }
}