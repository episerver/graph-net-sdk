//using EPiServer.Find;
//using EPiServer.Find.Api.Querying;
//using EPiServer.Find.Api.Querying.Filters;
//using EPiServer.Find.Cms;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Extensions;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework.BestBets;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.Helpers;
using EPiServer.Find.Statistics;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Globalization;
using EPiServer.Security;
using Foundation.Features.CatalogContent;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Category;
using Foundation.Features.Media;
using Foundation.Features.MyOrganization.QuickOrderPage;
using Foundation.Features.MyOrganization.Users;
using Foundation.Features.NewProducts;
using Foundation.Features.Sales;
using Foundation.Features.Search.Category;
using Foundation.Infrastructure.Find;
using Foundation.Infrastructure.Find.Facets;
using Geta.Optimizely.Categories;
using Geta.Optimizely.Categories.Find.Extensions;
using Mediachase.Commerce.Security;
using Mediachase.Commerce.Website.Search;
using Foundation.Extensions;
using static Foundation.Features.Shared.SelectionFactories.InclusionOrderingSelectionFactory;
using Models = Optimizely.ContentGraph.DataModels;
using EPiServer.Core.Internal;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.ContentGraph.Api.Facets;

namespace Foundation.Features.Search
{
    public interface ISearchService
    {
        ProductSearchResults Search(IContent currentContent, FilterOptionViewModel filterOptions, string selectedFacets, int catalogId = 0);
        ProductSearchResults SearchWithFilters(IContent currentContent, FilterOptionViewModel filterOptions, IEnumerable<Filter> filters, int catalogId = 0);
        IEnumerable<ProductTileViewModel> SearchOnSale(SalesPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12);
        IEnumerable<ProductTileViewModel> SearchNewProducts(NewProductsPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12);
        IEnumerable<ProductTileViewModel> QuickSearch(string query, int catalogId = 0);
        IEnumerable<ProductTileViewModel> QuickSearch(FilterOptionViewModel filterOptions, int catalogId = 0);
        IEnumerable<SortOrder> GetSortOrder();
        string GetOutline(string nodeCode);
        IEnumerable<UserSearchResultModel> SearchUsers(string query, int page = 1, int pageSize = 50);
        IEnumerable<SkuSearchResultModel> SearchSkus(string query);
        ContentGraphSearchViewModel SearchContent(FilterOptionViewModel filterOptions);
        ContentGraphSearchViewModel SearchPdf(FilterOptionViewModel filterOptions);
        CategorySearchResults SearchByCategory(Pagination pagination);
        //TypeQueryBuilder<T> FilterByCategories<T>(TypeQueryBuilder<T> query, IEnumerable<ContentReference> categories) where T : ICategorizableContent;
    }

    public class SearchService : ISearchService
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly IContentLanguageAccessor _contentLanguageAccessor;
        //private readonly IClient _findClient;
        private readonly GraphQueryBuilder _graphClient;
        private readonly IFacetRegistry _facetRegistry;
        private const int DefaultPageSize = 18;
        //private readonly IFindUIConfiguration _findUIConfiguration;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentRepository _contentRepository;
        private readonly IPriceService _priceService;
        private readonly IPromotionService _promotionService;
        private readonly ICurrencyService _currencyservice;
        private readonly IContentLoader _contentLoader;
        private readonly IBestBetRepository _bestBetRepository;
        private static readonly Random _random = new Random();

        public SearchService(ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            IContentLanguageAccessor contentLanguageAccessor,
            //IClient findClient,
            GraphQueryBuilder graphQuery,
            IFacetRegistry facetRegistry,
            //IFindUIConfiguration findUIConfiguration,
            ReferenceConverter referenceConverter,
            IContentRepository contentRepository,
            IPriceService priceService,
            IPromotionService promotionService,
            ICurrencyService currencyservice,
            IContentLoader contentLoader,
            IBestBetRepository bestBetRepository
            )
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _contentLanguageAccessor = contentLanguageAccessor;
            //_findClient = findClient;
            _facetRegistry = facetRegistry;
            //_findUIConfiguration = findUIConfiguration;
            //_findClient.Personalization().Refresh();
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
            _priceService = priceService;
            _promotionService = promotionService;
            _currencyservice = currencyservice;
            _contentLoader = contentLoader;
            _bestBetRepository = bestBetRepository;
            _graphClient = graphQuery;
        }

        public ProductSearchResults Search(IContent currentContent,
            FilterOptionViewModel filterOptions,
            string selectedFacets,
            int catalogId = 0) => filterOptions == null ? CreateEmptyResult() : GetSearchResults(currentContent, filterOptions, selectedFacets, null, catalogId);

        public ProductSearchResults SearchWithFilters(IContent currentContent,
            FilterOptionViewModel filterOptions,
            IEnumerable<Filter> filters,
            int catalogId = 0) => filterOptions == null ? CreateEmptyResult() : GetSearchResults(currentContent, filterOptions, "", filters, catalogId);

        public IEnumerable<ProductTileViewModel> QuickSearch(FilterOptionViewModel filterOptions,
            int catalogId = 0)
            => string.IsNullOrEmpty(filterOptions.Q) ? Enumerable.Empty<ProductTileViewModel>() : GetSearchResults(null, filterOptions, "", null, catalogId).ProductViewModels;

        public IEnumerable<ProductTileViewModel> QuickSearch(string query, int catalogId = 0)
        {
            var filterOptions = new FilterOptionViewModel
            {
                Q = query,
                PageSize = 5,
                Sort = string.Empty,
                FacetGroups = new List<FacetGroupOption>(),
                Page = 1,
                TrackData = false
            };
            return QuickSearch(filterOptions, catalogId);
        }

        public IEnumerable<SortOrder> GetSortOrder()
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();

            return new List<SortOrder>
            {
                //new SortOrder {Name = ProductSortOrder.PriceAsc, Key = IndexingHelper.GetPriceField(market.MarketId, currency), SortDirection = SortDirection.Ascending},
                new SortOrder {Name = ProductSortOrder.Popularity, Key = "", SortDirection = SortDirection.Ascending},
                new SortOrder {Name = ProductSortOrder.NewestFirst, Key = "created", SortDirection = SortDirection.Descending}
            };
        }
        //TODO:search users not sure it's correct
        public IEnumerable<UserSearchResultModel> SearchUsers(string query, int page = 1, int pageSize = 50)
        {
            //var searchQuery = _findClient.Search<UserSearchResultModel>();
            var searchQuery = _graphClient.ForType<UsersPage>();
            if (!string.IsNullOrEmpty(query))
            {
                searchQuery = searchQuery.Search(query);
            }
            var results = searchQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .GetResultAsync<UserSearchResultModel>()
                .Result.Content;
            if (results != null && results.Hits.Any())
            {
                return results.Hits.AsEnumerable().Select(x => new UserSearchResultModel());
            }

            return Enumerable.Empty<UserSearchResultModel>();
        }

        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();

            //var results = _findClient.Search<GenericProduct>()
            var results = _graphClient.ForType<GenericProduct>()
                //.Filter(_ => _.VariationModels(), x => x.Code.PrefixCaseInsensitive(query))
                .Filter(x=> x.Code.MatchPrefix(query))
                //.FilterMarket(market)
                .Filter(x => x.Language.Name.Match(_contentLanguageAccessor.Language.Name))
                //.Track()
                .FilterForVisitor()
                .Fields(_ => _.VariationModels())
                .GetResultAsync<GenericProduct>()
                .Result.Content.Hits
                .ToList();

            if (results != null && results.Any())
            {
                return results.Select(variation =>
                {
                    var defaultPrice = _priceService.GetDefaultPrice(market.MarketId, DateTime.Now,
                        new CatalogKey(variation.Code), currency);
                    var discountedPrice = defaultPrice != null ? _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId,
                        currency) : null;
                    return new SkuSearchResultModel
                    {
                        Sku = variation.Code,
                        ProductName = string.IsNullOrEmpty(variation.Name) ? "" : variation.Name,
                        UnitPrice = discountedPrice?.UnitPrice.Amount ?? 0,
                        UrlImage = variation.DefaultImageUrl()
                    };
                });
            }
            return Enumerable.Empty<SkuSearchResultModel>();
        }

        public IEnumerable<ProductTileViewModel> SearchOnSale(SalesPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12)
        {
            //var market = _currentMarket.GetCurrentMarket();
            //var currency = _currencyService.GetCurrentCurrency();
            //var query = BaseInlcusionExclusionQuery(currentContent, catalogId);
            //query = query.Filter(x => (x as GenericProduct).OnSale.Match(true));
            //var result = query.GetContentResult();
            //var searchProducts = CreateProductViewModels(result, currentContent, "").ToList();
            //GetManaualInclusion(searchProducts, currentContent, market, currency);
            //pages = GetPages(currentContent, page, searchProducts.Count);
            //return searchProducts;
            pages = new List<int>();
            return null;
        }

        public IEnumerable<ProductTileViewModel> SearchNewProducts(NewProductsPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12)
        {
            //var market = _currentMarket.GetCurrentMarket();
            //var currency = _currencyService.GetCurrentCurrency();
            //var query = BaseInlcusionExclusionQuery(currentContent, page, pageSize, catalogId);
            //query = query.OrderByDescending(x => x.Created);
            //query = query.Take(currentContent.NumberOfProducts == 0 ? 12 : currentContent.NumberOfProducts);
            //var result = query.GetContentResult();
            //var searchProducts = CreateProductViewModels(result, currentContent, "").ToList();
            //GetManaualInclusion(searchProducts, currentContent, market, currency);
            //pages = GetPages(currentContent, page, searchProducts.Count);
            //return searchProducts;
            pages = new List<int>();
            return null;
        }

        public ContentGraphSearchViewModel SearchContent(FilterOptionViewModel filterOptions)
        {
            var model = new ContentGraphSearchViewModel
            {
                FilterOption = filterOptions
            };

            if (!filterOptions.Q.IsNullOrEmpty())
            {
                var siteId = SiteDefinition.Current.Id;
                //var query = _findClient.UnifiedSearchFor(filterOptions.Q, _findClient.Settings.Languages.GetSupportedLanguage(ContentLanguage.PreferredCulture) ?? Language.None)
                var query = _graphClient
                    .ForType<Models.FoundationPageData>()
                    .Search(filterOptions.Q)
                    .Fields(_=>_.Name, _=>_.Url, _=>_.PageImage.Url, _=> _.MainIntro)
                    .Locales(ContentLanguage.PreferredCulture)
                    .UsingSynonyms()
                    .Total()
                    .Facet(x => x.ContentType.FacetFilters(filterOptions.SectionFilter))
                    //.FilterFacet("AllSections", x => x.SearchSection.Exists())
                    //.Filter(x => (x.MatchTypeHierarchy(typeof(FoundationPageData)) & (((FoundationPageData)x).SiteId().Match(siteId.ToString())) | (x.MatchTypeHierarchy(typeof(PageData)) & x.MatchTypeHierarchy(typeof(MediaData)))))
                    //.Filter(x => x.ContentType.Eq("FoundationPageData") & x.SiteId.Eq(siteId.ToString()) | (x.ContentType.Eq("PageData") & x.ContentType.Eq("MediaData")))
                    .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                    .Take(filterOptions.PageSize)
                    //.ApplyBestBets();
                    ;
                var andFilter = BooleanFilter.AndFilter<Models.FoundationPageData>();
                andFilter.And(x => x.ContentType.Eq(typeof(FoundationPageData).Name) & x.SiteId.Eq(siteId.ToString()) | (x.ContentType.Eq(typeof(PageData).Name) & x.ContentType.Eq(typeof(MediaData).Name)));
                andFilter.And(x => x.ExcludeFromSearch.FieldExists(false) | x.ExcludeFromSearch.Eq(false));
                query.Filter(andFilter);
                //Include images in search results
                if (!filterOptions.IncludeImagesContent)
                {
                    query = query.Filter(x => x.ContentType.NotEq(typeof(ImageMediaData).Name));
                }
                //Exclude content from search
                //query = query.Filter(x => !(x as FoundationPageData).ExcludeFromSearch.Exists() | (x as FoundationPageData).ExcludeFromSearch.Match(false));
                //query = query.Filter(x => x.ExcludeFromSearch.FieldExists(false) | x.ExcludeFromSearch.Eq(false));

                // obey DNT
                //var doNotTrackHeader = HttpContextHelper.Current.HttpContext.Request.Headers["DNT"].ToString();
                //if ((doNotTrackHeader == null || doNotTrackHeader.Equals("0")) && filterOptions.TrackData)
                //{
                //    query = query.Track();
                //}

                //if (!string.IsNullOrWhiteSpace(filterOptions.SectionFilter))
                //{
                //    query = query.FilterHits(x => x.SearchSection.Match(filterOptions.SectionFilter));
                //}

                var hitSpec = new HitSpecification
                {
                    HighlightTitle = true,
                    HighlightExcerpt = true
                };

                //model.Hits = query.GetResult(hitSpec);
                //TODO: hightlight is developing
                var results = query.GetResultAsync<Models.FoundationPageData>().Result;
                if (results.Errors.IsNull())
                {
                    model.Hits = results.Content.Hits.Select(x => new ContentGraphSearchViewModel.SearchHit { Title = x.Name, Url = x.Url, ImageUri = x.PageImage.Url, Excerpt = x.MainIntro });
                    model.Facets = results.Content.Facets["ContentType"].Select(fc => new ContentGraphSearchViewModel.SearchFacet { Name = fc.Name, Count = fc.Count });
                    filterOptions.TotalCount = results.Content.Total;
                }
            }

            return model;
        }

        public ContentGraphSearchViewModel SearchPdf(FilterOptionViewModel filterOptions)
        {
            var model = new ContentGraphSearchViewModel
            {
                FilterOption = filterOptions
            };

            if (!filterOptions.Q.IsNullOrEmpty())
            {
                var siteId = SiteDefinition.Current.Id;
                //var query = _findClient.UnifiedSearchFor(filterOptions.Q, _findClient.Settings.Languages.GetSupportedLanguage(ContentLanguage.PreferredCulture) ?? Language.None)
                var query = _graphClient
                    .ForType<Models.FoundationPdfFile>()
                    .Locales(ContentLanguage.PreferredCulture)
                    .UsingSynonyms()
                    //.TermsFacetFor(x => x.SearchSection)
                    .Facet(x => x.ContentType.FacetFilters(filterOptions.SectionFilter))
                    //.FilterFacet("AllSections", x => x.SearchSection.Exists())
                    //.Filter(x => x.MatchTypeHierarchy(typeof(FoundationPdfFile)) | x.MatchTypeHierarchy(typeof(EPiServer.PdfPreview.Models.PdfFile)))
                    .Filter(x => (x.ContentType.Eq(typeof(FoundationPdfFile).Name)) | (x.ContentType.Eq(typeof(EPiServer.PdfPreview.Models.PdfFile).Name)) )
                    .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                    .Take(filterOptions.PageSize)
                    //.ApplyBestBets()
                    ;

                // obey DNT
                //var doNotTrackHeader = HttpContextHelper.Current.HttpContext.Request.Headers["DNT"].ToString();
                //if ((doNotTrackHeader == null || doNotTrackHeader.Equals("0")) && filterOptions.TrackData)
                //{
                //    query = query.Track();
                //}

                //if (!string.IsNullOrWhiteSpace(filterOptions.SectionFilter))
                //{
                //    query = query.FilterHits(x => x.SearchSection.Match(filterOptions.SectionFilter));
                //}

                //var hitSpec = new HitSpecification
                //{
                //    HighlightTitle = true,
                //    HighlightExcerpt = true
                //};

                //model.Hits = query.GetResult(hitSpec);
                var contents = query.GetResultAsync<Models.FoundationPdfFile>().Result.Content;
                model.Hits = contents.Hits.Select(x => new ContentGraphSearchViewModel.SearchHit { Title = x.Title, Url = x.Url });
                model.Facets = contents.Facets["ContentType"].Select(x => new ContentGraphSearchViewModel.SearchFacet { Name = x.Name, Count = x.Count });
                filterOptions.TotalCount = contents.Total;
            }

            return model;
        }

        public CategorySearchResults SearchByCategory(Pagination pagination)
        {
            if (pagination == null)
            {
                pagination = new Pagination();
            }

            //var query = _findClient.Search<FoundationPageData>();
            var query = _graphClient.ForType<Models.FoundationPageData>();
            //query = query.FilterByCategories(pagination.Categories);
            query = query.Filter(_=>_.Categories.FilterByCategories(pagination.Categories));

            if (pagination.Sort == CategorySorting.PublishedDate.ToString())
            {
                if (pagination.SortDirection.ToLower() == "asc")
                {
                    query = query.OrderBy(x => x.StartPublish);
                }
                else
                {
                    query = query.OrderBy(x => x.StartPublish, EPiServer.ContentGraph.Api.OrderMode.DESC);
                }
            }

            if (pagination.Sort == CategorySorting.Name.ToString())
            {
                if (pagination.SortDirection.ToLower() == "asc")
                {
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderBy(x => x.Name, EPiServer.ContentGraph.Api.OrderMode.DESC);
                }
            }

            query = query.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);

            var results = query.GetResultAsync<Models.FoundationPageData>().Result.Content;
            var model = new CategorySearchResults
            {
                Pagination = pagination,
                RelatedPages = results.Hits
            };
            model.Pagination.TotalMatching = results.Total;
            model.Pagination.TotalPage = (model.Pagination.TotalMatching / pagination.PageSize) + (model.Pagination.TotalMatching % pagination.PageSize > 0 ? 1 : 0);

            return model;
        }

        public TypeQueryBuilder<T> FilterByCategories<T>(TypeQueryBuilder<T> query, IEnumerable<Models.ContentModelReference> categories) where T : ICategorizableContent => 
            query.Filter("Categories.Id", new NumericFilterOperators().In(categories.Select(x=>x.Id).ToArray()));

        private List<int> GetPages(BaseInclusionExclusionPage currentContent, int page, int count)
        {
            var pages = new List<int>();

            if (!currentContent.AllowPaging)
            {
                return pages;
            }

            var totalPages = (count + currentContent.PageSize - 1) / currentContent.PageSize;
            pages = new List<int>();
            var startPage = page > 2 ? page - 2 : 1;
            for (var p = startPage; p < Math.Min((totalPages >= 5 ? startPage + 5 : startPage + totalPages), totalPages + 1); p++)
            {
                pages.Add(p);
            }
            return pages;
        }

        private static List<T> Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = _random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        private void GetManaualInclusion(List<ProductTileViewModel> results,
            BaseInclusionExclusionPage baseInclusionExclusionPage,
            IMarket market,
            Currency currency)
        {
            var currentCount = results.Count;
            if (baseInclusionExclusionPage.ManualInclusion == null || !baseInclusionExclusionPage.ManualInclusion.Any())
            {
                return;
            }

            var inclusions = GetManualInclusion(baseInclusionExclusionPage.ManualInclusion).Select(x => x.GetProductTileViewModel(market, currency));
            if (baseInclusionExclusionPage.ManualInclusionOrdering == InclusionOrdering.Beginning)
            {
                results.InsertRange(0, inclusions);
                results = results.Take(baseInclusionExclusionPage.NumberOfProducts).ToList();
            }
            else
            {
                var total = currentCount + inclusions.Count();
                if (total > baseInclusionExclusionPage.NumberOfProducts)
                {
                    var num = baseInclusionExclusionPage.NumberOfProducts - inclusions.Count();
                    results = results.Take(num < 0 ? 0 : num).ToList();
                    results.AddRange(inclusions);
                }

                results = results.Take(baseInclusionExclusionPage.NumberOfProducts).ToList();

                if (baseInclusionExclusionPage.ManualInclusionOrdering == InclusionOrdering.Random)
                {
                    results = Shuffle(results);
                }
                else
                {
                    results.AddRange(inclusions);
                }
            }
        }

        private TypeQueryBuilder<Models.FoundationPageData> BaseInlcusionExclusionQuery<T>(T currentContent, int page = 0, int pageSize = 12, int catalogId = 0) where T : BaseInclusionExclusionPage
        {
            var market = _currentMarket.GetCurrentMarket();
            var query = _graphClient.ForType<Models.FoundationPageData>();
            //query = query.FilterMarket(market);
            query = query.Filter(x => x.Language.Name.Match(_contentLanguageAccessor.Language.Name));
            query = query.FilterForVisitor();
            //if (catalogId != 0)
            //{
            //    query = query.Filter(x => x.CatalogId.Match(catalogId));
            //}

            //Manual Exclusion
            if (currentContent.ManualExclusion != null && currentContent.ManualExclusion.Any())
            {
                query = ApplyManualExclusion(query, null);
            }

            //return query.StaticallyCacheFor(TimeSpan.FromMinutes(1))
            //    .Skip((page <= 0 ? 0 : page - 1) * pageSize)
            //    .Take(pageSize);
            return query
               .Skip((page <= 0 ? 0 : page - 1) * pageSize)
               .Take(pageSize);
        }

        private TypeQueryBuilder<Models.FoundationPageData> ApplyManualExclusion(TypeQueryBuilder<Models.FoundationPageData> query, IList<Models.ContentModelReference> manualExclusion)
        {
            //foreach (var item in _contentLoader.GetItems(manualExclusion, _contentLanguageAccessor.Language))
            //{
            //    if (item.GetOriginalType().Equals(typeof(EPiServer.Commerce.Catalog.ContentTypes.CatalogContent)))
            //    {
            //        query = query.Filter(x => !x.CatalogId.Match(((EPiServer.Commerce.Catalog.ContentTypes.CatalogContent)item).CatalogId));
            //    }
            //    else if (item.GetOriginalType().Equals(typeof(GenericNode)))
            //    {
            //        query = query.Filter(x => !x.Ancestors().Match(item.ContentLink.ToString()));
            //    }
            //    else if (item.GetOriginalType().Equals(typeof(GenericProduct))
            //        || item.GetOriginalType().Equals(typeof(GenericPackage)))
            //    {
            //        query = query.Filter(x => !x.ContentGuid.Match(item.ContentGuid));
            //    }
            //}

            return query;
        }

        private IEnumerable<EntryContentBase> GetManualInclusion(IList<ContentReference> manualInclusion)
        {
            //var results = new List<EntryContentBase>();
            //foreach (var item in _contentLoader.GetItems(manualInclusion, _contentLanguageAccessor.Language))
            //{
            //    if (item.GetOriginalType().Equals(typeof(EPiServer.Commerce.Catalog.ContentTypes.CatalogContent)))
            //    {
            //        results.AddRange(_findClient.Search<EntryContentBase>()
            //           .Filter(_ => _.CatalogId.Match(((EPiServer.Commerce.Catalog.ContentTypes.CatalogContent)item).CatalogId))
            //           .GetContentResult());
            //    }
            //    else if (item.GetOriginalType().Equals(typeof(GenericNode)))
            //    {
            //        results.AddRange(_findClient.Search<EntryContentBase>()
            //           .Filter(_ => _.Ancestors().Match(((GenericNode)item).ContentLink.ToString()))
            //           .GetContentResult());
            //    }
            //    else if (item.GetOriginalType().Equals(typeof(GenericProduct))
            //        || item.GetOriginalType().Equals(typeof(GenericPackage)))
            //    {
            //        results.Add(item as EntryContentBase);
            //    }
            //}
            //return ListExtensions.DistinctBy(results, (e) => e.ContentGuid);
            return null;
        }

        private ProductSearchResults GetSearchResults(IContent currentContent,
            FilterOptionViewModel filterOptions,
            string selectedfacets,
            IEnumerable<Filter> filters = null,
            int catalogId = 0)
        {
            //If contact belong organization, only find product that belong the categories that has owner is this organization
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var organizationId = contact?.ContactOrganization?.PrimaryKeyId ?? Guid.Empty;
            EPiServer.Commerce.Catalog.ContentTypes.CatalogContent catalogOrganization = null;
            if (organizationId != Guid.Empty)
            {
                //get category that has owner id = organizationId
                catalogOrganization = _contentRepository
                    .GetChildren<EPiServer.Commerce.Catalog.ContentTypes.CatalogContent>(_referenceConverter.GetRootLink())
                    .FirstOrDefault(x => !string.IsNullOrEmpty(x.Owner) && x.Owner.Equals(organizationId.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            var pageSize = filterOptions.PageSize > 0 ? filterOptions.PageSize : DefaultPageSize;
            var market = _currentMarket.GetCurrentMarket();

            //var query = _findClient.Search<EntryContentBase>();
            var query = _graphClient.ForType<Models.GenericProduct>();
            //query = ApplyTermFilter(query, filterOptions.Q, filterOptions.TrackData);
            query = query
                .Search(filterOptions.Q)
                .UsingSynonyms();
            query = query.Filter(x => x.Language.Name.Eq(_contentLanguageAccessor.Language.Name));

            if (organizationId != Guid.Empty && catalogOrganization != null)
            {
                //TODO: dont know outline filter
                //query = query.Filter(x => x.Outline().PrefixCaseInsensitive(catalogOrganization.Name));
            }

            var nodeContent = currentContent as NodeContent;
            if (nodeContent != null)
            {
                var outline = GetOutline(nodeContent.Code);
                //TODO: dont know outline filter
                //query = query.FilterOutline(new[] { outline });
            }

            //query = query.FilterMarket(market);
            //query = query.Filter(market.MarketId.Value, new StringFilterOperators().StartWith(market.MarketId.Value));
            var facetQuery = query;

            query = FilterSelected(query, filterOptions.FacetGroups);
            query = ApplyFilters(query, filters);
            if ((filterOptions.Sort == "Position" || filterOptions.Sort == "Recommended")
                    && filterOptions.SortDirection == "Asc")
            {
                query = query.Where(x => x.Boost, new NumericFilterOperators().Eq(2).Boost(1));
                query = query.Where(x => x.Boost, new NumericFilterOperators().Eq(3).Boost(2));
                query = query.Where(x => x.Boost, new NumericFilterOperators().Eq(4).Boost(3));
                query = query.Where(x => x.Boost, new NumericFilterOperators().Eq(5).Boost(4));
                //query = query.ThenByScore();
            }
            else
            {
                query = OrderBy(query, filterOptions);
            }

            //Exclude products from search
            query = query.Filter(x => x.Bury.Eq(false));

            if (catalogId != 0)
            {
                query = query.Filter(x => x.CatalogId.Eq(catalogId));
            }

            query = query
                //.ApplyBestBets()
                //.PublishedInCurrentLanguage() //TODO: Not support yet
                .FilterForVisitor()
                .Skip((filterOptions.Page - 1) * pageSize)
                .Total()
                .Take(pageSize);
            //.StaticallyCacheFor(TimeSpan.FromMinutes(1)); //TODO: Not support yet
            var result = query.GetResultAsync<Models.GenericProduct>().Result;
            if (result.Errors == null && result.Content?.Hits != null)
            {
                return new ProductSearchResults
                {
                    ProductViewModels = CreateProductViewModels(result.Content.Hits, currentContent, filterOptions.Q),
                    FacetGroups = GetFacetResults(filterOptions.FacetGroups, facetQuery, selectedfacets),
                    TotalCount = result.Content.Total,
                    DidYouMeans = null,
                    Query = filterOptions.Q,
                };
            }
            
            return new ProductSearchResults()
            {
                ProductViewModels = new List<ProductTileViewModel>(),
                TotalCount = 0,
                FacetGroups = new List<FacetGroupOption>()
            };
        }

        public IEnumerable<ProductTileViewModel> CreateProductViewModels(IEnumerable<Models.GenericProduct> searchResult, IContent content, string searchQuery)
        {
            List<ProductTileViewModel> productViewModels = null;
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();

            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            productViewModels = searchResult.Select(d => new ProductTileViewModel { Brand = d.Brand, Code = d.Code, DisplayName = d.DisplayName, Description = d.Description}).ToList();
            ApplyBoostedProperties(ref productViewModels, searchResult, content, searchQuery);
            return productViewModels;
        }

        public virtual StringCollection GetOutlinesForNode(string code)
        {
            var nodes = SearchFilterHelper.GetOutlinesForNode(code);
            if (nodes.Count == 0)
            {
                return nodes;
            }
            nodes[nodes.Count - 1] = nodes[nodes.Count - 1].Replace("*", "");
            return nodes;
        }

        public virtual string GetOutline(string nodeCode) => GetOutlineForNode(nodeCode);

        private string GetOutlineForNode(string nodeCode)
        {
            if (string.IsNullOrEmpty(nodeCode))
            {
                return "";
            }
            var outline = nodeCode;
            var currentNode = _contentRepository.Get<NodeContent>(_referenceConverter.GetContentLink(nodeCode));
            var parent = _contentRepository.Get<CatalogContentBase>(currentNode.ParentLink);
            while (!ContentReference.IsNullOrEmpty(parent.ParentLink))
            {
                var catalog = parent as EPiServer.Commerce.Catalog.ContentTypes.CatalogContent;
                if (catalog != null)
                {
                    outline = string.Format("{1}/{0}", outline, catalog.Name);
                }

                var parentNode = parent as NodeContent;
                if (parentNode != null)
                {
                    outline = string.Format("{1}/{0}", outline, parentNode.Code);
                }

                parent = _contentRepository.Get<CatalogContentBase>(parent.ParentLink);
            }
            return outline;
        }

        //private static ITypeSearch<EntryContentBase> ApplyTermFilter(ITypeSearch<EntryContentBase> query, string searchTerm, bool trackData)
        //{
        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        return query;
        //    }

        //    query = query.For(searchTerm).UsingSynonyms();
        //    if (trackData)
        //    {
        //        query = query.Track();
        //    }

        //    return query;
        //}

        private TypeQueryBuilder<Models.GenericProduct> OrderBy(TypeQueryBuilder<Models.GenericProduct> query, FilterOptionViewModel commerceFilterOptionViewModel)
        {
            if (string.IsNullOrEmpty(commerceFilterOptionViewModel.Sort) || commerceFilterOptionViewModel.Sort.Equals("Position"))
            {
                if (commerceFilterOptionViewModel.SortDirection.Equals("Asc"))
                {
                    query = query.OrderBy(x => x.Code);
                    return query;
                }
                query = query.OrderBy(x => x.Code, EPiServer.ContentGraph.Api.OrderMode.DESC);
                return query;
            }

            //if (commerceFilterOptionViewModel.Sort.Equals("Price"))
            //{
            //    if (commerceFilterOptionViewModel.SortDirection.Equals("Asc"))
            //    {
            //        query = query.OrderBy(x => x.DefaultPrice());
            //        query = query.ThenByScore();
            //        return query;
            //    }
            //    query = query.OrderByDescending(x => x.DefaultPrice());
            //    return query;
            //}

            if (commerceFilterOptionViewModel.Sort.Equals("Name"))
            {
                if (commerceFilterOptionViewModel.SortDirection.Equals("Asc"))
                {
                    query = query.OrderBy(x => x.DisplayName);
                    return query;
                }
                query = query.OrderBy(x => x.DisplayName, EPiServer.ContentGraph.Api.OrderMode.DESC);
                return query;
            }

            //if (CommerceFilterOptionViewModel.Sort.Equals("Recommended"))
            //{
            //    query = query.UsingPersonalization();
            //    return query;
            //}

            return query;
        }

        private IEnumerable<FacetGroupOption> GetFacetResults(List<FacetGroupOption> options,
            TypeQueryBuilder<Models.GenericProduct> query,
            string selectedfacets)
        {
            if (options == null)
            {
                return Enumerable.Empty<FacetGroupOption>();
            }

            var facets = _facetRegistry.GetFacetDefinitions();
            var facetGroups = facets.Select(x => new FacetGroupOption
            {
                GroupFieldName = x.FieldName,
                GroupName = x.DisplayName,
            }).ToList();

            query = facets.Aggregate(query, (current, facet) => current.Facet(GetSelectedFilter(options, facet.FieldName)));

            var productFacetsResult = query.Take(0).GetResultAsync<Models.GenericProduct>().Result.Content;
            if (productFacetsResult.Facets == null)
            {
                return facetGroups;
            }

            foreach (var facetGroup in facetGroups)
            {
                var filter = facets.FirstOrDefault(x => x.FieldName.Equals(facetGroup.GroupFieldName));
                if (filter == null)
                {
                    continue;
                }

                productFacetsResult.Facets.TryGetValue(facetGroup.GroupFieldName, out var facet);
                if (facet == null)
                {
                    continue;
                }

                filter.PopulateFacet(facetGroup, facet, selectedfacets);
            }
            return facetGroups;
        }

        private List<FacetFilter> GetSelectedFilter(List<FacetGroupOption> options, string currentField)
        {
            var filters = new List<FacetFilter>();
            var facets = _facetRegistry.GetFacetDefinitions();
            foreach (var facetGroupOption in options)
            {
                StringFacetFilterOperators facetFilter = new StringFacetFilterOperators();

                if (facetGroupOption.GroupFieldName.Equals(currentField))
                {
                    continue;
                }

                var filter = facets.FirstOrDefault(x => x.FieldName.Equals(facetGroupOption.GroupFieldName));
                if (filter == null)
                {
                    continue;
                }

                if (!facetGroupOption.Facets.Any(x => x.Selected))
                {
                    continue;
                }

                if (filter is FacetStringDefinition)
                {
                    //filters.Add(new TermsFilter(_findClient.GetFullFieldName(facetGroupOption.GroupFieldName, typeof(string)),
                    //    facetGroupOption.Facets.Where(x => x.Selected).Select(x => FieldFilterValue.Create(x.Name))));

                    facetFilter.Filters(facetGroupOption.Facets.Where(x => x.Selected).Select(x => x.Name).ToArray());
                    filters.Add(new TermFacetFilter(facetGroupOption.GroupFieldName, facetFilter));
                }
                else if (filter is FacetStringListDefinition)
                {
                    //var termFilters = facetGroupOption.Facets.Where(x => x.Selected)
                    //    .Select(s => new TermFacetFilter(facetGroupOption.GroupFieldName, FieldFilterValue.Create(s.Name)))
                    //    .Cast<Filter>()
                    //    .ToList();

                    facetFilter.Filters(facetGroupOption.Facets.Where(x => x.Selected).Select(x => x.Name).ToArray());
                    filters.Add(new TermFacetFilter(facetGroupOption.GroupFieldName, facetFilter));
                }
                else if (filter is FacetNumericRangeDefinition)
                {
                    var rangeFilters = filter as FacetNumericRangeDefinition;
                    NumericFacetFilterOperators numericFacetFilter = new NumericFacetFilterOperators();
                    List<(double?, double?)> ranges = new List<(double?, double?)>();

                    foreach (var selectedRange in facetGroupOption.Facets.Where(x => x.Selected))
                    {
                        var rangeFilter = rangeFilters.Range.FirstOrDefault(x => x.Id.Equals(selectedRange.Key.Split(':')[1]));
                        if (rangeFilter == null)
                        {
                            continue;
                        }
                        //filters.Add(RangeFilter.Create(_findClient.GetFullFieldName(facetGroupOption.GroupFieldName, typeof(double)),
                        //    rangeFilter.From ?? 0,
                        //    rangeFilter.To ?? double.MaxValue));
                        ranges.Add((rangeFilter.From, rangeFilter.To));
                    }

                    numericFacetFilter.Ranges(ranges.ToArray());
                    filters.Add(new TermFacetFilter(facetGroupOption.GroupFieldName, numericFacetFilter));
                }
            }

            if (!filters.Any())
            {
                return null;
            }

            return filters;
        }

        private TypeQueryBuilder<T> FilterSelected<T>(TypeQueryBuilder<T> query, List<FacetGroupOption> options)
        {
            var facets = _facetRegistry.GetFacetDefinitions();

            foreach (var facetGroupOption in options)
            {
                var filter = facets.FirstOrDefault(x => x.FieldName.Equals(facetGroupOption.GroupFieldName));
                if (filter == null)
                {
                    continue;
                }

                if (facetGroupOption.Facets != null && !facetGroupOption.Facets.Any(x => x.Selected))
                {
                    continue;
                }

                if (filter is FacetStringDefinition)
                {
                    var names = facetGroupOption.Facets
                        .Where(x => x.Selected)
                        .Select(x => x.Name).ToList();
                    query = query.Filter(filter.FieldName, new StringFilterOperators().In(names.ToArray()));
                }
                else if (filter is FacetStringListDefinition)
                {
                    var names = facetGroupOption.Facets
                        .Where(x => x.Selected)
                        .Select(x => x.Name).ToList();

                    query = query.Filter(filter.FieldName.In(names.ToArray()));
                }
                else if (filter is FacetNumericRangeDefinition)
                {
                    var numericFilter = filter as FacetNumericRangeDefinition;
                    var ranges = new List<SelectableNumericRange>();
                    var myRanges = new List<(double?,double?)>();
                    var selectedFacets = facetGroupOption.Facets.Where(x => x.Selected);
                    foreach (var facetOption in selectedFacets)
                    {
                        var range = numericFilter.Range.FirstOrDefault(x => x.Id.Equals(facetOption.Key.Split(':')[1]));
                        if (range == null)
                        {
                            continue;
                        }
                        ranges.Add(new SelectableNumericRange
                        {
                            From = range.From,
                            Id = range.Id,
                            Selected = range.Selected,
                            To = range.To
                        });
                        myRanges.Add((range.From, range.To));
                    }

                    //query = numericFilter.Filter(query, ranges);
                    query = query.Filter(numericFilter.FieldName, new NumericFilterOperators().InRanges(myRanges.ToArray()));
                }
            }
            return query;
        }

        private TypeQueryBuilder<Models.GenericProduct> ApplyFilters(TypeQueryBuilder<Models.GenericProduct> query,
            IEnumerable<Filter> filters)
        {
            if (filters == null || !filters.Any())
            {
                return query;
            }

            foreach (var filter in filters)
            {
                query = query.Filter(filter);
            }
            return query;
        }

        private static ProductSearchResults CreateEmptyResult()
        {
            return new ProductSearchResults
            {
                ProductViewModels = Enumerable.Empty<ProductTileViewModel>(),
                FacetGroups = Enumerable.Empty<FacetGroupOption>(),
            };
        }

        ///// <summary>
        ///// Sets Featured Product property and Best Bet Product property to ProductViewModels.
        ///// </summary>
        ///// <param name="productViewModels">The ProductViewModels is added two properties: Featured Product and Best Bet.</param>
        ///// <param name="searchResult">The search result (product list).</param>
        ///// <param name="currentContent">The product category.</param>
        ///// <param name="searchQuery">The search query string to filter Best Bet result.</param>
        private void ApplyBoostedProperties(ref List<ProductTileViewModel> productViewModels, IEnumerable<Models.GenericProduct> searchResult, IContent currentContent, string searchQuery)
        {
            var node = currentContent as GenericNode;
            var products = new List<EntryContentBase>();

            if (node != null)
            {
                UpdateListWithFeatured(ref productViewModels, node);
            }

            var bestBetList = _bestBetRepository.List().Where(i => i.PhraseCriterion.Phrase.CompareTo(searchQuery) == 0);
            //Filter for product best bet only.
            var productBestBet = bestBetList.Where(i => i.BestBetSelector is CommerceBestBetSelector);
            var ownStyleBestBet = bestBetList.Where(i => i.BestBetSelector is CommerceBestBetSelector && i.HasOwnStyle);
            productViewModels.ToList()
                             .ForEach(p =>
                             {
                                 if (productBestBet.Any(i => ((CommerceBestBetSelector)i.BestBetSelector).ContentLink.ID == p.ProductId))
                                 {
                                     p.IsBestBetProduct = true;
                                 }
                                 if (ownStyleBestBet.Any(i => ((CommerceBestBetSelector)i.BestBetSelector).ContentLink.ID == p.ProductId))
                                 {
                                     p.HasBestBetStyle = true;
                                 }
                             });
        }

        private void UpdateListWithFeatured(ref List<ProductTileViewModel> productViewModels, GenericNode node)
        {
            if (!node.FeaturedProducts?.FilteredItems?.Any() ?? true)
            {
                return;
            }
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            var index = 0;
            foreach (var item in node.FeaturedProducts.FilteredItems)
            {
                var content = item.GetContent();
                if (content is EntryContentBase featuredEntry)
                {
                    if (productViewModels.Any(x => x.Code.Equals(featuredEntry.Code)))
                    {
                        productViewModels.RemoveAt(productViewModels.IndexOf(productViewModels.First(x => x.Code.Equals(featuredEntry.Code))));
                    }
                    else
                    {
                        productViewModels.RemoveAt(productViewModels.IndexOf(productViewModels.Last()));
                    }

                    productViewModels.Insert(index, featuredEntry.GetProductTileViewModel(market, currency, true));
                    index++;
                }
                else if (content is GenericNode featuredNode)
                {
                    foreach (var nodeEntry in _contentLoader.GetChildren<EntryContentBase>(content.ContentLink)
                        .Where(x => !(x is VariationContent))
                        .Take(featuredNode.PartialPageSize))
                    {
                        if (productViewModels.Any(x => x.Code.Equals(nodeEntry.Code)))
                        {
                            productViewModels.RemoveAt(productViewModels.IndexOf(productViewModels.First(x => x.Code.Equals(nodeEntry.Code))));
                        }
                        else
                        {
                            productViewModels.RemoveAt(productViewModels.IndexOf(productViewModels.Last()));
                        }
                        productViewModels.Insert(index, nodeEntry.GetProductTileViewModel(market, currency, true));
                        index++;
                    }
                }
            }
        }
    }
}