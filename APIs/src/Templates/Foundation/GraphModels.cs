using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.SpecializedProperties;
using EPiServer.DataAnnotations;
using System.Globalization;

namespace Optimizely.ContentGraph.DataModels
{
    public class GenericNodeSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class ContentLanguageModel
    {
        public string Link { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
    public class BundleContentSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
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
    public class BundleContentRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class BundleContentAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class NodeContentSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class NodeContentCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class ProductContentSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class ProductContentCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class ProductContentRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class ProductContentAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class VariationContentSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class VariationContentCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class VariationContentRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class VariationContentAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class VariationContentShippingDimensions
    {
        public float Length { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
    public class PackageContentSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class PackageContentCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class PackageContentRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class PackageContentAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class PackageContentShippingDimensions
    {
        public float Length { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
    public class GenericBundleSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class GenericBundleCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericBundleRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericBundleAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class CommunityPageCommentsBlock
    {
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public int CommentBoxRows { get; set; }
        public int CommentMaxLength { get; set; }
        public int CommentsDisplayMax { get; set; }
        public bool SendActivity { get; set; }
    }
    public class CommunityPageRatingBlock
    {
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public bool SendActivity { get; set; }
    }
    public class CommunityPageSubscriptionBlock
    {
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
    }
    public class CommunityPageMembershipDisplayBlock
    {
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public string GroupName { get; set; }
        public int NumberOfMembers { get; set; }
    }
    public class CommunityPageGroupAdmissionBlock
    {
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public string GroupName { get; set; }
    }
    public class GenericVariantSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class GenericVariantCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericVariantRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericVariantAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericVariantShippingDimensions
    {
        public float Length { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
    public class GenericProductSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class GenericProductCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericProductRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericProductAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericPackageSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class GenericPackageCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericPackageRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericPackageAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericPackageShippingDimensions
    {
        public float Length { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
    public class DynamicVariantSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class DynamicVariantCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class DynamicVariantRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class DynamicVariantAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class DynamicVariantShippingDimensions
    {
        public float Length { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
    public class DynamicProductSeoInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
    public class DynamicProductCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class DynamicProductRelations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class DynamicProductAssociations
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class HeroBlockHeroBlockCallout
    {
        public string CalloutContent { get; set; }
        public string CalloutContentAlignment { get; set; }
        public string CalloutTextColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool BackgroundColorBehindText { get; set; }
        public float CalloutOpacity { get; set; }
        public string CalloutPosition { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
    }
    public class ProductHeroBlockProductHeroBlockCallout
    {
        public string Padding { get; set; }
        public string Margin { get; set; }
        public string BackgroundColor { get; set; }
        public string Text { get; set; }
    }
    public class InlineBlockPropertyModel
    {
        public IEnumerable<string> ContentType { get; set; }
    }
    public class CarouselBlockCarouselControls
    {
        public bool ShowControls { get; set; }
        public bool ShowIndicators { get; set; }
        public bool AutoPlay { get; set; }
        public bool Fade { get; set; }
        public int Interval { get; set; }
        public string Theme { get; set; }
    }
    public class CallToActionBlockButtonBlock
    {
        public bool ButtonTextUppercase { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public bool TextColorOverdrive { get; set; }
        public bool BackgroundColorOverdrive { get; set; }
        public bool BorderStyleOverdrive { get; set; }
        public string ButtonText { get; set; }
        public string TextPadding { get; set; }
        public bool ShowTransparentBackground { get; set; }
        public string BorderStyle { get; set; }
        public string ButtonLink { get; set; }
        public int FontSize { get; set; }
        public string ButtonBackgroundColor { get; set; }
        public int BorderWidth { get; set; }
        public int BorderRadius { get; set; }
        public string ButtonStyle { get; set; }
        public string ButtonBackgroundHoverColor { get; set; }
        public string ButtonBorderColor { get; set; }
        public string ButtonCaption { get; set; }
        public string ButtonBorderHoverColor { get; set; }
        public string ButtonTextColor { get; set; }
        public string ButtonTextHoverColor { get; set; }
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
    public class BundleContentCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class GenericNodeCategories
    {
        public ContentModelReference ContentLink { get; set; }
    }
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class ContentAreaItemModel
    {
        public string DisplayOption { get; set; }
        public string Tag { get; set; }
        public ContentModelReference ContentLink { get; set; }
        public InlineBlockPropertyModel InlineBlock { get; set; }
    }
    public class ProductHeroBlockProductHeroBlockImage
    {
        public IEnumerable<ContentAreaItemModel> Product { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string ImagePosition { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
    }
    public class LinkItemNode
    {
        public string Href { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string Text { get; set; }
        public ContentModelReference ContentLink { get; set; }
    }
    public class BlobModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }
    public class Decimal
    {
    }
    public class FormContainerBlockModel
    {
        public string Template { get; set; }
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
    public class ProductSearchBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string SearchTerm { get; set; }
        public int ResultsPerPage { get; set; }
        public int ItemsPerRow { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> Nodes { get; set; }
        [Searchable]
        public string SortOrder { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> Filters { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> PriorityProducts { get; set; }
        [Searchable]
        public string DiscontinuedProductsMode { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        [Searchable]
        public string BrandFilter { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SubOrganizationPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class QuickOrderPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> QuickOrderBlockContentArea { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrganizationPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrdersPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BudgetingPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SubscriptionHistoryPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SubscriptionDetailPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ResetPasswordMailPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string Subject { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class UsersPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ResetPasswordPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrderHistoryPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrderDetailsPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrderConfirmationPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string Title { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Body { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> RegistrationArea { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GiftCardPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CreditCardPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public bool B2B { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BookmarksPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class AddressBookPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CodingFile:Content
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
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class ProfilePage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FoundationPdfFile:Content
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
        public bool DisplayAsPreview { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowIcon { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int Height { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class CartPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public bool ShowRecommendations { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> BottomContentArea { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SharedCartPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class StorePage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        public string TitleColor { get; set; }
        public string BackgroundColor { get; set; }
        public float BackgroundOpacity { get; set; }
        public ContentModelReference BackgroundImage { get; set; }
        public ContentModelReference BackgroundVideo { get; set; }
        public string TopPaddingMode { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrderPadsPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SearchResultPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        public bool ShowRecommendations { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GenericNode:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> FeaturedProducts { get; set; }
        [Searchable]
        public string LongName { get; set; }
        [Searchable]
        public string Teaser { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        public int PartialPageSize { get; set; }
        public bool ShowRecommendations { get; set; }
        [Searchable]
        public string DefaultTemplate { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public GenericNodeSeoInformation SeoInformation { get; set; }
        public GenericNodeCategories Categories { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SalesPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        public int NumberOfProducts { get; set; }
        public int PageSize { get; set; }
        public bool AllowPaging { get; set; }
        public IEnumerable<ContentModelReference> ManualInclusion { get; set; }
        [Searchable]
        public string ManualInclusionOrdering { get; set; }
        public IEnumerable<ContentModelReference> ManualExclusion { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PersonList:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PersonPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string JobTitle { get; set; }
        [Searchable]
        public string Location { get; set; }
        [Searchable]
        public string Sector { get; set; }
        [Searchable]
        public string Phone { get; set; }
        [Searchable]
        public string Email { get; set; }
        public ContentModelReference Image { get; set; }
        [Searchable]
        public string About { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class NewProductsPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        public int NumberOfProducts { get; set; }
        public int PageSize { get; set; }
        public bool AllowPaging { get; set; }
        public IEnumerable<ContentModelReference> ManualInclusion { get; set; }
        [Searchable]
        public string ManualInclusionOrdering { get; set; }
        public IEnumerable<ContentModelReference> ManualExclusion { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class WishListPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ImageMediaData:Content
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
        public BlobModel LargeThumbnail { get; set; }
        [Searchable]
        public string ImageAlignment { get; set; }
        [Searchable]
        public string FileSize { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        [Searchable]
        public string AccentColor { get; set; }
        [Searchable]
        public string Caption { get; set; }
        [Searchable]
        public string ClipArtType { get; set; }
        [Searchable]
        public string DominantColorBackground { get; set; }
        [Searchable]
        public string DominantColorForeground { get; set; }
        public IEnumerable<string> DominantColors { get; set; }
        public IEnumerable<string> ImageCategories { get; set; }
        public bool IsAdultContent { get; set; }
        public bool IsBwImg { get; set; }
        public bool IsRacyContent { get; set; }
        [Searchable]
        public string LineDrawingType { get; set; }
        public IEnumerable<string> Tags { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string AltText { get; set; }
        [Searchable]
        public string CreditsText { get; set; }
        public string CreditsLink { get; set; }
        public ContentModelReference Link { get; set; }
        [Searchable]
        public string Copyright { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class StandardFile:Content
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
        public string FileSize { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string Description { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowIcon { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class VectorImageMediaData:Content
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
        public BlobModel LargeThumbnail { get; set; }
        [Searchable]
        public string ImageAlignment { get; set; }
        [Searchable]
        public string FileSize { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        [Searchable]
        public string AccentColor { get; set; }
        [Searchable]
        public string Caption { get; set; }
        [Searchable]
        public string ClipArtType { get; set; }
        [Searchable]
        public string DominantColorBackground { get; set; }
        [Searchable]
        public string DominantColorForeground { get; set; }
        public IEnumerable<string> DominantColors { get; set; }
        public IEnumerable<string> ImageCategories { get; set; }
        public bool IsAdultContent { get; set; }
        public bool IsBwImg { get; set; }
        public bool IsRacyContent { get; set; }
        [Searchable]
        public string LineDrawingType { get; set; }
        public IEnumerable<string> Tags { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string AltText { get; set; }
        [Searchable]
        public string CreditsText { get; set; }
        public string CreditsLink { get; set; }
        public ContentModelReference Link { get; set; }
        [Searchable]
        public string Copyright { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class OrderConfirmationMailPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string Subject { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ReportingMediaData:Content
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
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class BundleContent:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public ContentModelReference BundleReference { get; set; }
        public BundleContentSeoInformation SeoInformation { get; set; }
        public BundleContentCategories Categories { get; set; }
        public BundleContentRelations ParentEntries { get; set; }
        public BundleContentAssociations Associations { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CatalogContent:Content
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
        public string DefaultCurrency { get; set; }
        public string DefaultLanguage { get; set; }
        public bool IsPrimary { get; set; }
        public string Owner { get; set; }
        public string WeightBase { get; set; }
        public string LengthBase { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CheckoutPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class NodeContent:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public NodeContentSeoInformation SeoInformation { get; set; }
        public NodeContentCategories Categories { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ProductContent:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public ContentModelReference VariantsReference { get; set; }
        public ProductContentSeoInformation SeoInformation { get; set; }
        public ProductContentCategories Categories { get; set; }
        public ProductContentRelations ParentEntries { get; set; }
        public ProductContentAssociations Associations { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class VariationContent:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public float MinQuantity { get; set; }
        public int TaxCategoryId { get; set; }
        public VariationContentSeoInformation SeoInformation { get; set; }
        public VariationContentCategories Categories { get; set; }
        public VariationContentRelations ParentEntries { get; set; }
        public VariationContentAssociations Associations { get; set; }
        public float MaxQuantity { get; set; }
        public ContentModelReference PriceReference { get; set; }
        public string SeoUri { get; set; }
        public float Weight { get; set; }
        [Searchable]
        public string Code { get; set; }
        public int ShippingPackageId { get; set; }
        public VariationContentShippingDimensions ShippingDimensions { get; set; }
        public bool TrackInventory { get; set; }
        public ContentModelReference InventoryReference { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class DAMAsset:Content
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
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class DAMImageAsset:Content
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
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class DAMVideoAsset:Content
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
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class PdfFile:Content
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
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class FoundationPageData:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PackageContent:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public float MinQuantity { get; set; }
        public int TaxCategoryId { get; set; }
        public ContentModelReference PackageReference { get; set; }
        public PackageContentSeoInformation SeoInformation { get; set; }
        public PackageContentCategories Categories { get; set; }
        public PackageContentRelations ParentEntries { get; set; }
        public PackageContentAssociations Associations { get; set; }
        public float MaxQuantity { get; set; }
        public ContentModelReference PriceReference { get; set; }
        public string SeoUri { get; set; }
        public float Weight { get; set; }
        [Searchable]
        public string Code { get; set; }
        public int ShippingPackageId { get; set; }
        public PackageContentShippingDimensions ShippingDimensions { get; set; }
        public bool TrackInventory { get; set; }
        public ContentModelReference InventoryReference { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BlogItemPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string Author { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BlogListPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public ContentModelReference Root { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public bool IncludeAllLevels { get; set; }
        public int SortOrder { get; set; }
        public bool IncludePublishDate { get; set; }
        public bool IncludeTeaserText { get; set; }
        public string TeaserRatio { get; set; }
        public IEnumerable<ContentModelReference> CategoryListFilter { get; set; }
        [Searchable]
        public string Template { get; set; }
        [Searchable]
        public string PreviewOption { get; set; }
        [Searchable]
        public string OverlayColor { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public string OverlayTextColor { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GenericBundle:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string LongDescription { get; set; }
        public bool OnSale { get; set; }
        public bool NewArrival { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea { get; set; }
        [Searchable]
        public string AssociationsTitle { get; set; }
        public bool ShowRecommendations { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public ContentModelReference BundleReference { get; set; }
        public GenericBundleSeoInformation SeoInformation { get; set; }
        public GenericBundleCategories Categories { get; set; }
        public GenericBundleRelations ParentEntries { get; set; }
        public GenericBundleAssociations Associations { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
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
        public ContentModelReference PreviewImage { get; set; }
        [Searchable]
        public string Copyright { get; set; }
        public bool DisplayControls { get; set; }
        public bool Autoplay { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class WebImageMediaData:Content
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
        public BlobModel LargeThumbnail { get; set; }
        [Searchable]
        public string ImageAlignment { get; set; }
        [Searchable]
        public string FileSize { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        [Searchable]
        public string AccentColor { get; set; }
        [Searchable]
        public string Caption { get; set; }
        [Searchable]
        public string ClipArtType { get; set; }
        [Searchable]
        public string DominantColorBackground { get; set; }
        [Searchable]
        public string DominantColorForeground { get; set; }
        public IEnumerable<string> DominantColors { get; set; }
        public IEnumerable<string> ImageCategories { get; set; }
        public bool IsAdultContent { get; set; }
        public bool IsBwImg { get; set; }
        public bool IsRacyContent { get; set; }
        [Searchable]
        public string LineDrawingType { get; set; }
        public IEnumerable<string> Tags { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string AltText { get; set; }
        [Searchable]
        public string CreditsText { get; set; }
        public string CreditsLink { get; set; }
        public ContentModelReference Link { get; set; }
        [Searchable]
        public string Copyright { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
        [Searchable]
        public string Content { get; set; }
    }
    public class TagPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> Images { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> BottomArea { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class LocationListPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> FilterArea { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class LocationItemPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public string Continent { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Country { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float AvgTemp { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string AirportInitials { get; set; }
        public int YearlyPassengers { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public ContentModelReference Image { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> LeftContentArea { get; set; }
        public bool New { get; set; }
        public bool Promoted { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class TwoColumnLandingPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> RightContentArea { get; set; }
        public int LeftColumn { get; set; }
        public int RightColumn { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ThreeColumnLandingPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> LeftContentArea { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> RightContentArea { get; set; }
        public int LeftColumn { get; set; }
        public int CenterColumn { get; set; }
        public int RightColumn { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class HomePage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> TopContentArea { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> BottomContentArea { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FolderPage:Content
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
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CalendarEventPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public DateTime EventStartDate { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public DateTime EventEndDate { get; set; }
        [Searchable]
        public string Location { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CommunityPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        public CommunityPageCommentsBlock Comments { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public CommunityPageRatingBlock Ratings { get; set; }
        public CommunityPageSubscriptionBlock Subscriptions { get; set; }
        public CommunityPageMembershipDisplayBlock Memberships { get; set; }
        public CommunityPageGroupAdmissionBlock GroupAdmission { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CollectionPage:Content
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
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string MainIntro { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> Navigation { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string CollectionName { get; set; }
        public ContentModelReference Image { get; set; }
        public ContentModelReference Video { get; set; }
        [Searchable]
        public string Description { get; set; }
        public string TeaserRatio { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> Products { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string MetaTitle { get; set; }
        public bool ExcludeFromSearch { get; set; }
        public ContentModelReference PageImage { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public bool HideSiteHeader { get; set; }
        public ContentModelReference TeaserVideo { get; set; }
        public string Css { get; set; }
        [Searchable]
        public string PageDescription { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string MetaContentType { get; set; }
        [Searchable]
        public string Industry { get; set; }
        [Searchable]
        public string AuthorMetaData { get; set; }
        public bool DisableIndexing { get; set; }
        public bool Highlight { get; set; }
        public string TeaserTextAlignment { get; set; }
        public string TeaserColorTheme { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public string TeaserButtonStyle { get; set; }
        public bool ApplyHoverEffect { get; set; }
        public string Padding { get; set; }
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GenericVariant:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        [Searchable]
        public string Size { get; set; }
        [Searchable]
        public string Mpn { get; set; }
        [Searchable]
        public string Color { get; set; }
        [Searchable]
        public string PackageQuantity { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string PartNumber { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea { get; set; }
        [Searchable]
        public string RegionCode { get; set; }
        [Searchable]
        public string AssociationsTitle { get; set; }
        [Searchable]
        public string Sku { get; set; }
        public bool ShowRecommendations { get; set; }
        [Searchable]
        public string SubscriptionLength { get; set; }
        [Searchable]
        public string VirtualProductMode { get; set; }
        [Searchable]
        public string Upc { get; set; }
        [Searchable]
        public string VirtualProductRole { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public float MinQuantity { get; set; }
        public int TaxCategoryId { get; set; }
        public GenericVariantSeoInformation SeoInformation { get; set; }
        public GenericVariantCategories Categories { get; set; }
        public GenericVariantRelations ParentEntries { get; set; }
        public GenericVariantAssociations Associations { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public float MaxQuantity { get; set; }
        public ContentModelReference PriceReference { get; set; }
        public string SeoUri { get; set; }
        public float Weight { get; set; }
        [Searchable]
        public string Code { get; set; }
        public int ShippingPackageId { get; set; }
        public GenericVariantShippingDimensions ShippingDimensions { get; set; }
        public bool TrackInventory { get; set; }
        public ContentModelReference InventoryReference { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GenericProduct:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public int Boost { get; set; }
        public bool Bury { get; set; }
        [Searchable]
        public string Sizing { get; set; }
        [Searchable]
        public string Manufacturer { get; set; }
        [Searchable]
        public string ProductTeaser { get; set; }
        [Searchable]
        public string ManufacturerPartsWarrantyDescription { get; set; }
        [Searchable]
        public string Brand { get; set; }
        [Searchable]
        public string Model { get; set; }
        [Searchable]
        public string Department { get; set; }
        [Searchable]
        public string ModelYear { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Warranty { get; set; }
        [Searchable]
        public string LegalDisclaimer { get; set; }
        [Searchable]
        public string ProductGroup { get; set; }
        [Searchable]
        public string ProductTypeName { get; set; }
        [Searchable]
        public string ProductTypeSubcategory { get; set; }
        public bool OnSale { get; set; }
        public bool NewArrival { get; set; }
        [Searchable]
        public string LongDescription { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea { get; set; }
        [Searchable]
        public string AssociationsTitle { get; set; }
        public bool ShowRecommendations { get; set; }
        [Searchable]
        public string ProductStatus { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public ContentModelReference VariantsReference { get; set; }
        public GenericProductSeoInformation SeoInformation { get; set; }
        public GenericProductCategories Categories { get; set; }
        public GenericProductRelations ParentEntries { get; set; }
        public GenericProductAssociations Associations { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GenericPackage:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        [Searchable]
        public string Description { get; set; }
        public bool OnSale { get; set; }
        public bool NewArrival { get; set; }
        [Searchable]
        public string LongDescription { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea { get; set; }
        [Searchable]
        public string AssociationsTitle { get; set; }
        public bool ShowRecommendations { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public float MinQuantity { get; set; }
        public int TaxCategoryId { get; set; }
        public ContentModelReference PackageReference { get; set; }
        public GenericPackageSeoInformation SeoInformation { get; set; }
        public GenericPackageCategories Categories { get; set; }
        public GenericPackageRelations ParentEntries { get; set; }
        public GenericPackageAssociations Associations { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public float MaxQuantity { get; set; }
        public ContentModelReference PriceReference { get; set; }
        public string SeoUri { get; set; }
        public float Weight { get; set; }
        [Searchable]
        public string Code { get; set; }
        public int ShippingPackageId { get; set; }
        public GenericPackageShippingDimensions ShippingDimensions { get; set; }
        public bool TrackInventory { get; set; }
        public ContentModelReference InventoryReference { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class DynamicVariant:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        [Searchable]
        public string Size { get; set; }
        [Searchable]
        public string Mpn { get; set; }
        [Searchable]
        public string Color { get; set; }
        [Searchable]
        public string PackageQuantity { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string PartNumber { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea { get; set; }
        [Searchable]
        public string RegionCode { get; set; }
        [Searchable]
        public string AssociationsTitle { get; set; }
        [Searchable]
        public string Sku { get; set; }
        public bool ShowRecommendations { get; set; }
        [Searchable]
        public string SubscriptionLength { get; set; }
        [Searchable]
        public string VirtualProductMode { get; set; }
        [Searchable]
        public string Upc { get; set; }
        [Searchable]
        public string VirtualProductRole { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public float MinQuantity { get; set; }
        public int TaxCategoryId { get; set; }
        public DynamicVariantSeoInformation SeoInformation { get; set; }
        public DynamicVariantCategories Categories { get; set; }
        public DynamicVariantRelations ParentEntries { get; set; }
        public DynamicVariantAssociations Associations { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public float MaxQuantity { get; set; }
        public ContentModelReference PriceReference { get; set; }
        public string SeoUri { get; set; }
        public float Weight { get; set; }
        [Searchable]
        public string Code { get; set; }
        public int ShippingPackageId { get; set; }
        public DynamicVariantShippingDimensions ShippingDimensions { get; set; }
        public bool TrackInventory { get; set; }
        public ContentModelReference InventoryReference { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class DynamicProduct:Content
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
        [Searchable]
        public string DisplayName { get; set; }
        public int MetaClassId { get; set; }
        public int CatalogId { get; set; }
        public int Boost { get; set; }
        public bool Bury { get; set; }
        [Searchable]
        public string Sizing { get; set; }
        [Searchable]
        public string Manufacturer { get; set; }
        [Searchable]
        public string ProductTeaser { get; set; }
        [Searchable]
        public string ManufacturerPartsWarrantyDescription { get; set; }
        [Searchable]
        public string Brand { get; set; }
        [Searchable]
        public string Model { get; set; }
        [Searchable]
        public string Department { get; set; }
        [Searchable]
        public string ModelYear { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Warranty { get; set; }
        [Searchable]
        public string LegalDisclaimer { get; set; }
        [Searchable]
        public string ProductGroup { get; set; }
        [Searchable]
        public string ProductTypeName { get; set; }
        [Searchable]
        public string ProductTypeSubcategory { get; set; }
        public bool OnSale { get; set; }
        public bool NewArrival { get; set; }
        [Searchable]
        public string LongDescription { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ContentArea { get; set; }
        [Searchable]
        public string AssociationsTitle { get; set; }
        public bool ShowRecommendations { get; set; }
        [Searchable]
        public string ProductStatus { get; set; }
        public bool HideSiteHeader { get; set; }
        public IEnumerable<LinkItemNode> CssFiles { get; set; }
        public IEnumerable<LinkItemNode> ScriptFiles { get; set; }
        public ContentModelReference VariantsReference { get; set; }
        public DynamicProductSeoInformation SeoInformation { get; set; }
        public DynamicProductCategories Categories { get; set; }
        public DynamicProductRelations ParentEntries { get; set; }
        public DynamicProductAssociations Associations { get; set; }
        public bool HideSiteFooter { get; set; }
        [Searchable]
        public string Css { get; set; }
        [Searchable]
        public string Scripts { get; set; }
        public string SeoUri { get; set; }
        [Searchable]
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CustomViewConfigurationBlock:Content
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
        public ContentModelReference Root { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public ContentModelReference NewContentRoot { get; set; }
        [Searchable]
        public string AllowedTypesToAddString { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ShippingDimensions:Content
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
        public float Length { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class Relations:Content
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
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class StringFilterBlock:Content
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
        public string FieldName { get; set; }
        [Searchable]
        public string FieldValue { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool IncludePublishDate { get; set; }
        public bool IncludeTeaserText { get; set; }
        public int Count { get; set; }
        public int SortOrder { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> Roots { get; set; }
        public string PageTypeFilter { get; set; }
        public IEnumerable<ContentModelReference> CategoryListFilter { get; set; }
        public bool Recursive { get; set; }
        [Searchable]
        public string Template { get; set; }
        [Searchable]
        public string PreviewOption { get; set; }
        [Searchable]
        public string BootstrapCardRatioOption { get; set; }
        [Searchable]
        public string OverlayColor { get; set; }
        [Searchable]
        public string OverlayTextColor { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrderSearchBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class NavigationBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public ContentModelReference RootPage { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ModalBlock:Content
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
        public bool ShowModalOnPageLoad { get; set; }
        public bool ShowModalOnExitIntent { get; set; }
        public int ShowModalOnScrollPercentage { get; set; }
        public int ShowModalAfterXSeconds { get; set; }
        public bool ShowModalOpenButton { get; set; }
        [Searchable]
        public string ModalOpenButtonText { get; set; }
        public int ModalOpenButtonWidth { get; set; }
        public bool HideModalTitle { get; set; }
        [Searchable]
        public string ModalTitle { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ModalContentArea { get; set; }
        public bool HideModalFooter { get; set; }
        [Searchable]
        public string ModalCloseButtonText { get; set; }
        [Searchable]
        public string ModalPrimaryButtonText { get; set; }
        public string ModalPrimaryButtonLink { get; set; }
        public ContentModelReference ModalBackgroundImage { get; set; }
        public ContentModelReference ModalBackdropImage { get; set; }
        [Searchable]
        public string ModalSize { get; set; }
        [Searchable]
        public string CssClass { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class MenuItemBlock:Content
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
        public string Link { get; set; }
        public ContentModelReference MenuImage { get; set; }
        [Searchable]
        public string TeaserText { get; set; }
        [Searchable]
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class MembershipDisplayBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        [Searchable]
        public string GroupName { get; set; }
        public int NumberOfMembers { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class MembershipAffiliationBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public int NumberOfMembers { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class NumericFilterBlock:Content
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
        public string FieldName { get; set; }
        [Searchable]
        public string FieldOperator { get; set; }
        public float FieldValue { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class LikeButtonBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class HeroBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string BlockRatio { get; set; }
        public ContentModelReference BackgroundImage { get; set; }
        public ContentModelReference MainBackgroundVideo { get; set; }
        public string Link { get; set; }
        public HeroBlockHeroBlockCallout Callout { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SeoInformation:Content
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
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Keywords { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GroupCreationBlock:Content
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
        public bool ShowHeading { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GroupAdmissionBlock:Content
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
        public bool ShowHeading { get; set; }
        [Searchable]
        public string GroupName { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class GoogleMapsBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string ApiKey { get; set; }
        [Searchable]
        public string SearchTerm { get; set; }
        public float Height { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FeedBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public int FeedDisplayMax { get; set; }
        [Searchable]
        public string FeedTitle { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FacebookBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string AccountName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ElevatedRoleBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string Body { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class HeroBlockCallout:Content
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
        public string CalloutContent { get; set; }
        [Searchable]
        public string CalloutContentAlignment { get; set; }
        [Searchable]
        public string CalloutTextColor { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public bool BackgroundColorBehindText { get; set; }
        public float CalloutOpacity { get; set; }
        [Searchable]
        public string CalloutPosition { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ContainerBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> MainContentArea { get; set; }
        [Searchable]
        public string CssClass { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ExistsFilterBlock:Content
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
        public string FieldName { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ProductHeroBlockCallout:Content
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
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        [Searchable]
        public string Text { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class WidgetBlock:Content
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
        public string WidgetType { get; set; }
        public int NumberOfRecommendations { get; set; }
        [Searchable]
        public string Value { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class QuickOrderBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class OrderHistoryBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FilterActivitiesBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string FilterTitle { get; set; }
        [Searchable]
        public string AllConditionText { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FilterContinentsBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string FilterTitle { get; set; }
        [Searchable]
        public string AllConditionText { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FilterDistancesBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string FilterTitle { get; set; }
        [Searchable]
        public string AllConditionText { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FilterTemperaturesBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string FilterTitle { get; set; }
        [Searchable]
        public string AllConditionText { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CalendarBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string ViewMode { get; set; }
        public ContentModelReference EventsRoot { get; set; }
        public int Count { get; set; }
        public IEnumerable<CategoryModel> CategoryFilter { get; set; }
        public bool Recursive { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ProductHeroBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Layout { get; set; }
        public ProductHeroBlockProductHeroBlockCallout Callout { get; set; }
        public ProductHeroBlockProductHeroBlockImage Image { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class YouTubeBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string YouTubeLink { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class VideoBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        public ContentModelReference Video { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class TwitterBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string AccountName { get; set; }
        public int NumberOfItems { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class TextBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string TeaserButtonText { get; set; }
        public int HeadingSize { get; set; }
        [Searchable]
        public string HeadingStyle { get; set; }
        [Searchable]
        public string HeadingColor { get; set; }
        [Searchable]
        public string Description { get; set; }
        public string TeaserButtonStyle { get; set; }
        public ContentModelReference Link { get; set; }
        [Searchable]
        public string Text { get; set; }
        public ContentModelReference Image { get; set; }
        public int ImageSize { get; set; }
        public int MaxImageHeight { get; set; }
        public ContentModelReference SecondImage { get; set; }
        public int SecondImageSize { get; set; }
        [Searchable]
        public string TextColor { get; set; }
        public bool DisplayAsCard { get; set; }
        [Searchable]
        public string Height { get; set; }
        public int MinCardHeight { get; set; }
        public bool AlignButtonBottom { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SubscriptionBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class RssReaderBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public string RssUrl { get; set; }
        public int MaxCount { get; set; }
        public bool IncludePublishDate { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class RatingBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public bool SendActivity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ProductHeroBlockImage:Content
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
        public IEnumerable<ContentAreaItemModelSearch> Product { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        [Searchable]
        public string ImagePosition { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class VimeoBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public string VimeoVideoLink { get; set; }
        public ContentModelReference CoverImage { get; set; }
        [Searchable]
        public string Heading { get; set; }
        [Searchable]
        public string MainBody { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CommentsBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public bool ShowHeading { get; set; }
        public int CommentBoxRows { get; set; }
        public int CommentMaxLength { get; set; }
        public int CommentsDisplayMax { get; set; }
        public bool SendActivity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class HealthChatbotBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string HeaderText { get; set; }
        [Searchable]
        public string DirectLineToken { get; set; }
        public int HeightInPixels { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CarouselControls:Content
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
        public bool ShowControls { get; set; }
        public bool ShowIndicators { get; set; }
        public bool AutoPlay { get; set; }
        public bool Fade { get; set; }
        public int Interval { get; set; }
        [Searchable]
        public string Theme { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PromotionSchedule:Content
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
        public bool UseCampaignDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public ContentModelReference CampaignLink { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CategoryBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Heading { get; set; }
        public ContentModelReference Catalog { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class RedemptionLimitsData:Content
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
        public int PerPromotion { get; set; }
        public int PerOrderForm { get; set; }
        public int PerCustomer { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class DiscountItems:Content
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
        public IEnumerable<ContentModelReference> Items { get; set; }
        public bool MatchRecursive { get; set; }
        public int MaxQuantity { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class MonetaryReward:Content
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
        public float Percentage { get; set; }
        public bool UseAmounts { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PurchaseAmount:Content
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
        public float PartiallyFulfilledThreshold { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PurchaseQuantity:Content
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
        public float PartiallyFulfilledThreshold { get; set; }
        public int RequiredQuantity { get; set; }
        public IEnumerable<ContentModelReference> Items { get; set; }
        public bool MatchRecursive { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class Associations:Content
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
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FixedPricePurchaseQuantity:Content
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
        public float PartiallyFulfilledThreshold { get; set; }
        public int RequiredQuantity { get; set; }
        public IEnumerable<ContentModelReference> Items { get; set; }
        public bool MatchRecursive { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BreadcrumbBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public ContentModelReference DestinationPage { get; set; }
        [Searchable]
        public string Separator { get; set; }
        [Searchable]
        public string Alignment { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CarouselBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> CarouselItems { get; set; }
        public CarouselBlockCarouselControls CarouselControls { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
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
        public bool ButtonTextUppercase { get; set; }
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public bool TextColorOverdrive { get; set; }
        public bool BackgroundColorOverdrive { get; set; }
        public bool BorderStyleOverdrive { get; set; }
        [Searchable]
        public string ButtonText { get; set; }
        [Searchable]
        public string TextPadding { get; set; }
        public bool ShowTransparentBackground { get; set; }
        [Searchable]
        public string BorderStyle { get; set; }
        public string ButtonLink { get; set; }
        public int FontSize { get; set; }
        public string ButtonBackgroundColor { get; set; }
        public int BorderWidth { get; set; }
        public int BorderRadius { get; set; }
        [Searchable]
        public string ButtonStyle { get; set; }
        public string ButtonBackgroundHoverColor { get; set; }
        public string ButtonBorderColor { get; set; }
        [Searchable]
        public string ButtonCaption { get; set; }
        public string ButtonBorderHoverColor { get; set; }
        public string ButtonTextColor { get; set; }
        public string ButtonTextHoverColor { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CallToActionBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string Title { get; set; }
        [Searchable]
        public string Subtext { get; set; }
        [Searchable]
        public string TextColor { get; set; }
        public ContentModelReference BackgroundImage { get; set; }
        [Searchable]
        public string BackgroundImageSetting { get; set; }
        public CallToActionBlockButtonBlock Button { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class Categories:Content
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
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BootstrapCardBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        [Searchable]
        public string CardAlignment { get; set; }
        [Searchable]
        public string CardHeader { get; set; }
        [Searchable]
        public string CardTitle { get; set; }
        [Searchable]
        public string CardTitleSize { get; set; }
        [Searchable]
        public string CardSubtitle { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> CardContentArea { get; set; }
        [Searchable]
        public string CardFooter { get; set; }
        public ContentModelReference CardImage { get; set; }
        [Searchable]
        public string CardButtonText { get; set; }
        public string CardButtonLink { get; set; }
        public bool CardClickable { get; set; }
        public IEnumerable<LinkItemNode> CardLinks { get; set; }
        [Searchable]
        public string CssClass { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CouponData:Content
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
        public string Code { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ODPListConsentFormBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        [Searchable]
        public string PredefinedValue { get; set; }
        public int AutoComplete { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ODPListFormBlock:Content
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
        public string PredefinedValue { get; set; }
        [Searchable]
        public string ListId { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class AssetsDownloadLinksBlock:Content
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
        public IEnumerable<ContentModelReference> Categories { get; set; }
        [Searchable]
        public string Padding { get; set; }
        [Searchable]
        public string Margin { get; set; }
        [Searchable]
        public string BackgroundColor { get; set; }
        public float BlockOpacity { get; set; }
        public ContentModelReference RootContent { get; set; }
        public int Count { get; set; }
        [Searchable]
        public string GroupName { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class BlogCommentBlock:Content
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
        public int CommentsPerPage { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class TextboxElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        public int AutoComplete { get; set; }
        [Searchable]
        public string PredefinedValue { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class TextareaElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        public int AutoComplete { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class NumberElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        public int AutoComplete { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class AddressesElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string CountryLabel { get; set; }
        [Searchable]
        public string PostalLabel { get; set; }
        [Searchable]
        public string StateLabel { get; set; }
        [Searchable]
        public string CityLabel { get; set; }
        [Searchable]
        public string StreetLabel { get; set; }
        [Searchable]
        public string AddressLabel { get; set; }
        [Searchable]
        public string ApiKey { get; set; }
        public int MapHeight { get; set; }
        public int MapWidth { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class DateTimeElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int PickerType { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        [Searchable]
        public string PredefinedValue { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class DateTimeRangeElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int PickerType { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        [Searchable]
        public string PredefinedValue { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class RangeElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Step { get; set; }
        [Searchable]
        public string PredefinedValue { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class UrlElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        [Searchable]
        public string PredefinedValue { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SelectionElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        public bool AllowMultiSelect { get; set; }
        [Searchable]
        public string Feed { get; set; }
        [Searchable]
        public string PlaceHolder { get; set; }
        public int AutoComplete { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ChoiceElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        public bool AllowMultiSelect { get; set; }
        [Searchable]
        public string Feed { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ImageChoiceElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        public bool ShowSelectionInputControl { get; set; }
        public bool AllowMultiSelect { get; set; }
        public IEnumerable<LinkItemNode> Items { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FileUploadElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int FileSize { get; set; }
        [Searchable]
        public string FileTypes { get; set; }
        [Searchable]
        public string Validators { get; set; }
        public bool AllowMultiple { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class PredefinedHiddenElementBlock:Content
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
        public string PredefinedValue { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class VisitorDataHiddenElementBlock:Content
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
        public string VisitorDataSources { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class CaptchaElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Validators { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int TextLength { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class HcaptchaElementBlock:Content
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
        public string Validators { get; set; }
        public float ScoreThreshold { get; set; }
        [Searchable]
        public string SiteKey { get; set; }
        [Searchable]
        public string SecretKey { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class RecaptchaElementBlock:Content
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
        public string Validators { get; set; }
        public float ScoreThreshold { get; set; }
        [Searchable]
        public string SiteKey { get; set; }
        [Searchable]
        public string SecretKey { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ParagraphTextElementBlock:Content
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
        public string ParagraphText { get; set; }
        [Searchable]
        public string FormSubmissionId { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public bool DisablePlaceholdersReplacement { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FormStepBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        public string AttachedContentLink { get; set; }
        public ContentModelReference DependField { get; set; }
        public int DependCondition { get; set; }
        [Searchable]
        public string DependValue { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class SubmitButtonElementBlock:Content
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
        public bool FinalizeForm { get; set; }
        [Searchable]
        public string Label { get; set; }
        public string Image { get; set; }
        [Searchable]
        public string Description { get; set; }
        public string RedirectToPage { get; set; }
        [Searchable]
        public string SatisfiedAction { get; set; }
        public int ConditionCombination { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class ResetButtonElementBlock:Content
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
        public string Label { get; set; }
        [Searchable]
        public string Description { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
        public string SiteId { get; set; }
    }
    public class FormContainerBlock:Content
    {
        public FormContainerBlockModel FormModel { get; set; }
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
        public string MetadataAttribute { get; set; }
        [Searchable]
        public string Title { get; set; }
        public bool AllowToStoreSubmissionData { get; set; }
        [Searchable]
        public string Description { get; set; }
        public bool ShowSummarizedData { get; set; }
        [Searchable]
        public string ConfirmationMessage { get; set; }
        [Searchable]
        public string ResetConfirmationMessage { get; set; }
        public string RedirectToPage { get; set; }
        [Searchable]
        public string SubmitSuccessMessage { get; set; }
        public bool AllowAnonymousSubmission { get; set; }
        public bool AllowMultipleSubmission { get; set; }
        public bool ShowNavigationBar { get; set; }
        public bool AllowExposingDataFeeds { get; set; }
        [Searchable]
        public string PartialSubmissionRetentionPeriod { get; set; }
        [Searchable]
        public string FinalizedSubmissionRetentionPeriod { get; set; }
        [Searchable]
        public IEnumerable<ContentAreaItemModelSearch> ElementsArea { get; set; }
        public IEnumerable<string> Ancestors { get; set; }
        public bool IsCommonDraft { get; set; }
        public string RelativePath { get; set; }
        public string Shortcut { get; set; }
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
        public string Shortcut { get; set; }
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
