using EPiServer.ContentGraph.Api.Filters;
using Optimizely.ContentGraph.DataModels;

namespace Foundation.Extensions
{
    public static class FoudationFilterExtensions
    {
        public static DelegateFilterBuilder FilterByCategories(this IEnumerable<ContentModelReference> field, IEnumerable<ContentModelReference> categories)
        {
            return new DelegateFilterBuilder(field => new TermFilter("Categories.Id", new NumericFilterOperators().In(categories.Select(x => x.Id).ToArray())));
        }
        public static DelegateFilterBuilder InCategory(this IEnumerable<CategoryModel> category, CategoryModel value, int boostValue = 1)
        {
            return new DelegateFilterBuilder(field => new TermFilter("Category.Id", new NumericFilterOperators().In(value.Id).Boost(boostValue)));
        }
    }
}