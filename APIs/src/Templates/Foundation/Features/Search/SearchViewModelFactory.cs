//using EPiServer.Find;
//using EPiServer.Find.Cms;
//using EPiServer.Find.Commerce;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Extensions;
//using EPiServer.Find.Framework.BestBets;
using EPiServer.Framework.Cache;
using Foundation.Features.CatalogContent;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;
using Models = Optimizely.ContentGraph.DataModels;

namespace Foundation.Features.Search
{
    public interface ISearchViewModelFactory
    {
        SearchViewModel<TContent> Create<TContent>(TContent currentContent, string selectedFacets,
            int catlogId, FilterOptionViewModel filterOption)
            where TContent : IContent;
    }

    public class SearchViewModelFactory : ISearchViewModelFactory
    {
        private readonly ISearchService _searchService;
        private readonly LocalizationService _localizationService;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly UrlResolver _urlResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IClient _findClient;
        private readonly GraphQueryBuilder _findClient;
        private readonly ISynchronizedObjectInstanceCache _synchronizedObjectInstanceCache;

        public SearchViewModelFactory(LocalizationService localizationService, ISearchService searchService,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter,
            UrlResolver urlResolver,
            IHttpContextAccessor httpContextAccessor,
            //IClient findClient,
            GraphQueryBuilder findClient,
            ISynchronizedObjectInstanceCache synchronizedObjectInstanceCache)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
            _findClient = findClient;
            _synchronizedObjectInstanceCache = synchronizedObjectInstanceCache;
        }

        public virtual SearchViewModel<TContent> Create<TContent>(TContent currentContent,
            string selectedFacets,
            int catalogId,
            FilterOptionViewModel filterOption)
            where TContent : IContent
        {
            var model = new SearchViewModel<TContent>(currentContent);

            if (!filterOption.Q.IsNullOrEmpty() && (filterOption.Q.StartsWith("*") || filterOption.Q.StartsWith("?")))
            {
                model.CurrentContent = currentContent;
                model.FilterOption = filterOption;
                model.HasError = true;
                model.ErrorMessage = _localizationService.GetString("/Search/BadFirstCharacter");
                model.CategoriesFilter = new CategoriesFilterViewModel();
                return model;
            }

            var results = _searchService.Search(currentContent, filterOption, selectedFacets, catalogId);

            filterOption.TotalCount = results.TotalCount;
            filterOption.FacetGroups = results.FacetGroups.ToList();

            filterOption.Sorting = _searchService.GetSortOrder().Select(x => new SelectListItem
            {
                Text = _localizationService.GetString("/Category/Sort/" + x.Name),
                Value = x.Name.ToString(),
                Selected = string.Equals(x.Name.ToString(), filterOption.Sort)
            });

            model.CurrentContent = currentContent;
            model.ProductViewModels = results?.ProductViewModels ?? new List<ProductTileViewModel>();
            model.FilterOption = filterOption;
            model.CategoriesFilter = GetCategoriesFilter(currentContent, filterOption.Q);
            model.DidYouMeans = results.DidYouMeans;
            model.Query = filterOption.Q;
            var detection = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IDetectionService>();
            model.IsMobile = detection.Device.Type == Device.Mobile;

            return model;
        }

        private CategoriesFilterViewModel GetCategoriesFilter(IContent currentContent, string query)
        {
            //var bestBets = new BestBetRepository(_synchronizedObjectInstanceCache).List().Where(i => i.PhraseCriterion.Phrase.CompareTo(query) == 0);
            //var ownStyleBestBets = bestBets.Where(i => i.BestBetSelector is CommerceBestBetSelector && i.HasOwnStyle);
            var catalogId = 0;
            var node = currentContent as Models.NodeContent;
            if (node != null)
            {
                catalogId = node.CatalogId;
            }
            var catalog = _contentLoader.GetChildren<CatalogContentBase>(_referenceConverter.GetRootLink())
                .FirstOrDefault(x => catalogId == 0 || x.CatalogId == catalogId);

            if (catalog == null)
            {
                return new CategoriesFilterViewModel();
            }

            var viewModel = new CategoriesFilterViewModel();
            var nodes = _findClient
                .ForType<Models.NodeContent>()
                .Fields(x=>x.DisplayName, _=>_.ContentLink.Url)
                .Filter(x => x.ParentLink.Id.Eq(catalog.ContentLink.ID))
                .FilterForVisitor()
                .GetResultAsync<Models.NodeContent>()
                .Result
                .Content
                .Hits;

            foreach (var nodeContent in nodes)
            {
                var nodeFilter = new CategoryFilter
                {
                    DisplayName = nodeContent.DisplayName,
                    Url = _urlResolver.GetUrl(nodeContent.ContentLink.Url),
                    IsActive = currentContent != null && currentContent.ContentLink.GetUri().AbsoluteUri == nodeContent.ContentLink.Url,
                    IsBestBet = false//ownStyleBestBets.Any(x => ((CommerceBestBetSelector)x.BestBetSelector).ContentLink.ID == nodeContent.ContentLink.ID)
                };
                viewModel.Categories.Add(nodeFilter);

                GetChildrenNode(currentContent, nodeContent, nodeFilter);
            }
            return viewModel;
        }

        private void GetChildrenNode(IContent currentContent, Models.NodeContent node, CategoryFilter nodeFilter)
        {
            var nodeChildrenOfNode = 
                _findClient
                .ForType<Models.NodeContent>()
                .Filter(x => x.ParentLink.Id.Eq(node.ContentLink.Id))
                .Fields(x => x.DisplayName, _ => _.ContentLink.Url)
                .FilterForVisitor()
                .GetResultAsync<Models.NodeContent>()
                .Result
                .Content
                .Hits;
            foreach (var nodeChildOfChild in nodeChildrenOfNode)
            {
                var nodeChildOfChildFilter = new CategoryFilter
                {
                    DisplayName = nodeChildOfChild.DisplayName,
                    Url = _urlResolver.GetUrl(nodeChildOfChild.ContentLink.Url),
                    IsActive = currentContent != null && currentContent.ContentLink.GetUri().AbsoluteUri == nodeChildOfChild.ContentLink.Url,
                    IsBestBet = false//ownStyleBestBets.Any(x => ((CommerceBestBetSelector)x.BestBetSelector).ContentLink.ID == nodeChildOfChild.ContentLink.ID)
                };

                nodeFilter.Children.Add(nodeChildOfChildFilter);
                if (nodeChildOfChildFilter.IsActive)
                {
                    nodeFilter.IsActive = nodeFilter.IsActive = true;
                }

                GetChildrenNode(currentContent, nodeChildOfChild, nodeChildOfChildFilter);
            }
        }
    }
}
