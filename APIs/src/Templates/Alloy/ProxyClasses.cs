using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.SpecializedProperties;
using EPiServer.DataAnnotations;
using System.Globalization;
using System;

namespace Optimizely.ContentGraph.DataModels
{
    public class StartPageSiteLogotypeBlock
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }
    public class ContentLanguageModel
    {
        public string Link { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
    public class AllPropertiesTestPageEditorialBlock
    {
        public string MainBody { get; set; }
    }
    public class ContentModelReference
    {
        public int Id { get; set; }
        public int WorkId { get; set; }
        public string GuidValue { get; set; }
        public string ProviderName { get; set; }
        public string Url { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IContent Expanded { get; set; }
    }
    public class AllPropertiesTestPageListBlock
    {
        public IEnumerable<string> Tags { get; set; }
    }
    public class ContentRootsModel
    {
        public ContentModelReference GlobalAssetsRoot { get; set; }
        public ContentModelReference StartPage { get; set; }
        public ContentModelReference SiteAssetsRoot { get; set; }
        public ContentModelReference ContentAssetsRoot { get; set; }
        public ContentModelReference RootPage { get; set; }
        public ContentModelReference WasteBasket { get; set; }
    }
    public class AllPropertiesTestPageContactBlock
    {
        public ContentModelReference Image { get; set; }
        public string Heading { get; set; }
        public ContentModelReference ContactPageLink { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
    }
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class NewsPagePageListBlock
    {
        public string Heading { get; set; }
        public bool IncludePublishDate { get; set; }
        public bool IncludeIntroduction { get; set; }
        public int Count { get; set; }
        public int SortOrder { get; set; }
        public ContentModelReference Root { get; set; }
        public string PageTypeFilter { get; set; }
        public IEnumerable<CategoryModel> CategoryFilter { get; set; }
        public bool Recursive { get; set; }
    }
    public class BlobModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }
    public class InlineBlockPropertyModel
    {
        public IEnumerable<string> ContentType { get; set; }
    }
    public class ContentAreaItemModel
    {
        public string DisplayOption { get; set; }
        public string Tag { get; set; }
        public ContentModelReference ContentLink { get; set; }
        public InlineBlockPropertyModel InlineBlock { get; set; }
    }
    public class LinkItemNode
    {
        public string Href { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string Text { get; set; }
        public ContentModelReference ContentLink { get; set; }
    }
    public class XhtmlPropertyModel
    {
        public bool ExcludePersonalizedContent { get; set; }
        public string Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class NumberPropertyModel
    {
        public int Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class PageReferencePropertyModel
    {
        public IContent ExpandedValue { get; set; }
        public bool ExcludePersonalizedContent { get; set; }
        public ContentModelReference Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class ContentReferencePropertyModel
    {
        public IContent ExpandedValue { get; set; }
        public bool ExcludePersonalizedContent { get; set; }
        public ContentModelReference Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class GuidPropertyModel
    {
        public string Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class LinkItemPropertyModel
    {
        public IContent ExpandedValue { get; set; }
        public bool ExcludePersonalizedContent { get; set; }
        public LinkItemNode Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class UrlPropertyModel
    {
        public string Value { get; set; }
        public string PropertyDataType { get; set; }
    }
    public class SiteDefinitionLanguageModel
    {
        public bool IsMasterLanguage { get; set; }
        public string UrlSegment { get; set; }
        public string Url { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
    public class HostDefinitionModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ContentLanguageModel Language { get; set; }
    }
    public class ContentLanguageModelSearch
    {
        public string Link { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
    public class ContentModelReferenceSearch
    {
        public int Id { get; set; }
        public int WorkId { get; set; }
        public string GuidValue { get; set; }
        public string ProviderName { get; set; }
        public string Url { get; set; }
        public ContentLanguageModelSearch Language { get; set; }
        public IContent Expanded { get; set; }
    }
    public class InlineBlockPropertyModelSearch
    {
        public IEnumerable<string> ContentType { get; set; }
    }
    public class ContentAreaItemModelSearch
    {
        public string DisplayOption { get; set; }
        public string Tag { get; set; }
        public ContentModelReferenceSearch ContentLink { get; set; }
        public InlineBlockPropertyModelSearch InlineBlock { get; set; }
    }
    public class IngredientBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class VideoFile:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public BlobModel Thumbnail { get; set; }
        public string MimeType { get; set; }
        [Searchable]
        public string Copyright { get; set; }
        public ContentModelReference PreviewImage { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class ImageFile:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public BlobModel Thumbnail { get; set; }
        public string MimeType { get; set; }
        [Searchable]
        public string Copyright { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class GenericMedia:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public BlobModel Thumbnail { get; set; }
        public string MimeType { get; set; }
        [Searchable]
        public string Description { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class TagHelperPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        public LinkItemNode Link { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class StartPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public ContentModelReference GlobalNewsPageLink { get; set; }
        public ContentModelReference ContactsPageLink { get; set; }
        public ContentModelReference SearchPageLink { get; set; }
        public StartPageSiteLogotypeBlock SiteLogotype { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> ProductPageLinks { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> AllowCertainBlockTypes { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> RestrictCertainBlockTypes { get; set; }
        public IEnumerable<LinkItemNode> CompanyInformationPageLinks { get; set; }
        public IEnumerable<LinkItemNode> NewsPageLinks { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<LinkItemNode> CustomerZonePageLinks { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class NumberEditorTest:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public float Latitude { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class StandardPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class SearchPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> RelatedContentArea { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ProductPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        public IEnumerable<string> UniqueSellingPoints { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> RelatedContentArea { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class NewsPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        public NewsPagePageListBlock NewsList { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class LandingPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ContainerPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ArticlePage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ContactPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public ContentModelReference Image { get; set; }
        [Searchable]
        public string Phone { get; set; }
        [Searchable]
        public string Email { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<string> MetaKeywords { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        public bool HideSiteHeader { get; set; }
        [Searchable]
        public string MetaDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        public bool DisableIndexing { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class EditorialBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class VectorImageFile:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public BlobModel Thumbnail { get; set; }
        public string MimeType { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class TeaserBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string Text { get; set; }
        public ContentModelReference Image { get; set; }
        public ContentModelReference Link { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class SiteLogotypeBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string Title { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class PageListBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool IncludePublishDate { get; set; }
        public bool IncludeIntroduction { get; set; }
        public int Count { get; set; }
        public int SortOrder { get; set; }
        public ContentModelReference Root { get; set; }
        public string PageTypeFilter { get; set; }
        public IEnumerable<CategoryModel> CategoryFilter { get; set; }
        public bool Recursive { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class NestedBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentAreaItems { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ListBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class JumbotronBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public ContentModelReference Image { get; set; }
        [Searchable]
        public string ImageDescription { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string SubHeading { get; set; }
        [Searchable]
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class AllPropertiesTestPage:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string StringNoValidation { get; set; }
        [Searchable]
        public string StringLength { get; set; }
        [Searchable]
        public string StringRegex { get; set; }
        [Searchable]
        public string IntNoValidation { get; set; }
        [Searchable]
        public string Int { get; set; }
        public float FloatNoValidation { get; set; }
        public float Float { get; set; }
        public ContentModelReference ContentReference1 { get; set; }
        public ContentModelReference ContentReferenceReadonly1 { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea1 { get; set; }
        public IEnumerable<AllPropertiesTestPageEditorialBlock> EditorialBlocks { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentAreaReadonly1 { get; set; }
        public IEnumerable<string> XhtmlStrings { get; set; }
        public AllPropertiesTestPageContactBlock ContactBlock { get; set; }
        public IEnumerable<AllPropertiesTestPageContactBlock> ContactBlocks { get; set; }
        public ContentModelReference PageReference1 { get; set; }
        public ContentModelReference PageReference2 { get; set; }
        public ContentModelReference MediaReference1 { get; set; }
        public ContentModelReference MediaReference2 { get; set; }
        public ContentModelReference MediaReference3 { get; set; }
        public ContentModelReference MediaReference4 { get; set; }
        public ContentModelReference FolderReference { get; set; }
        public ContentModelReference MediaReference5 { get; set; }
        public ContentModelReference MediaReference6 { get; set; }
        public IEnumerable<int> EnumerableInts { get; set; }
        public ContentModelReference BlockReference1 { get; set; }
        public ContentModelReference BlockReference2 { get; set; }
        public ContentModelReference BlockReference3 { get; set; }
        public IEnumerable<int> EnumerableIntsPropertyList { get; set; }
        public IEnumerable<ContentModelReference> EnumerablePageReferencesPropertyList { get; set; }
        public IEnumerable<ContentModelReference> EnumerableContentReferencesPropertyList { get; set; }
        public IEnumerable<ContentModelReference> ContentReferenceList1 { get; set; }
        public IEnumerable<ContentModelReference> ContentReferenceListAllowedTypes { get; set; }
        public IEnumerable<int> ListInts { get; set; }
        public IEnumerable<ContentModelReference> ContentReferenceListReadonly1 { get; set; }
        public IEnumerable<string> ListStrings { get; set; }
        public IEnumerable<LinkItemNode> LinkItemCollection1 { get; set; }
        public IEnumerable<DateTime> Dates { get; set; }
        public IEnumerable<LinkItemNode> LinkItemCollectionReadonly1 { get; set; }
        public LinkItemNode LinkItem { get; set; }
        public LinkItemNode LinkItemReadonly { get; set; }
        public IEnumerable<string> Guids { get; set; }
        public string UrlReadonly { get; set; }
        public string UrlToImage { get; set; }
        [Searchable]
        public string Text1 { get; set; }
        public IEnumerable<LinkItemNode> LinksList { get; set; }
        [Searchable]
        public string TextReadonly1 { get; set; }
        public IEnumerable<LinkItemNode> LinksEnumerable { get; set; }
        [Searchable]
        public string TextArea1 { get; set; }
        public IEnumerable<ContentModelReference> Pages { get; set; }
        [Searchable]
        public string TextAreaReadonly1 { get; set; }
        public IEnumerable<string> Urls { get; set; }
        [Searchable]
        public string PreviewableText1 { get; set; }
        public IEnumerable<ContentModelReference> Images { get; set; }
        [Searchable]
        public string PreviewableTextReadonly1 { get; set; }
        public IEnumerable<ContentModelReference> Videos { get; set; }
        public DateTime Date1 { get; set; }
        public IEnumerable<string> TextAreas { get; set; }
        public DateTime DateReadonly1 { get; set; }
        public IEnumerable<ContentModelReference> References { get; set; }
        public int Integer1 { get; set; }
        public AllPropertiesTestPageListBlock ListBlock { get; set; }
        public int IntegerReadonly1 { get; set; }
        public int IntegerRange1 { get; set; }
        public bool Bool1 { get; set; }
        public bool BoolReadonly1 { get; set; }
        public string Guid { get; set; }
        public IEnumerable<int> IntegerList1 { get; set; }
        public IEnumerable<int> IntegerListReadonly1 { get; set; }
        public ContentModelReference Image1 { get; set; }
        public IEnumerable<string> StringListWithRegexValidation { get; set; }
        public IEnumerable<string> StringListWithMax10Characters { get; set; }
        public ContentModelReference ImageReadonly1 { get; set; }
        [Searchable]
        public string SingleSelect1 { get; set; }
        [Searchable]
        public string SingleSelectReadonly1 { get; set; }
        [Searchable]
        public string MultiSelect1 { get; set; }
        [Searchable]
        public string MultiSelectReadonly1 { get; set; }
        [Searchable]
        public string SelectionEditor1 { get; set; }
        [Searchable]
        public string SelectionEditor2 { get; set; }
        [Searchable]
        public string SelectionEditorReadonly1 { get; set; }
        public IEnumerable<string> CustomStringList { get; set; }
        public IEnumerable<int> IntList { get; set; }
        public string ListGeoCoordinate { get; set; }
        public ContentAreaItemModel ContentAreaItem { get; set; }
        public ContentAreaItemModel ContentAreaItemWithAllowedTypes { get; set; }
        public ContentAreaItemModel ContentAreaItemOnlyBlocks { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ContactBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public ContentModelReference Image { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public ContentModelReference ContactPageLink { get; set; }
        [Searchable]
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class ButtonBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class TestBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public string UrlToImageCode { get; set; }
        public string UrlToVideoCode { get; set; }
        public string UrlToMediaFileVideoCode { get; set; }
        public IEnumerable<string> ListUrl { get; set; }
        public IEnumerable<string> ListUrlToImageCode { get; set; }
        public IEnumerable<string> ListUrlToVideoCode { get; set; }
        public IEnumerable<string> ListUrlToMediaFileVideoCode { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class RecipeBlock:Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class Content
    {
        public ContentModelReference ContentLink { get; set; }
        [Searchable]
        public string Name { get; set; }
        public ContentLanguageModel Language { get; set; }
        public IEnumerable<ContentLanguageModel> ExistingLanguages { get; set; }
        public ContentLanguageModel MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public ContentModelReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string SiteId { get; set; }
    }
    public class SiteDefinition
    {
        public ContentModelReference ContentLink { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public string Status { get; set; }
        [Searchable]
        public string Name { get; set; }
        public string Id { get; set; }
        public string EditLocation { get; set; }
        public IEnumerable<SiteDefinitionLanguageModel> Languages { get; set; }
        public IEnumerable<HostDefinitionModel> Hosts { get; set; }
        public ContentRootsModel ContentRoots { get; set; }
    }
}
