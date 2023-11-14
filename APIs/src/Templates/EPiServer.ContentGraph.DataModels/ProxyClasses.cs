using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.SpecializedProperties;
using System.Globalization;

namespace EPiServer.ContentGraph.DataModels
{
    public class ContentLanguageModel
    {
        public string Link { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
    public class ContentLanguageModelSearch
    {
        public string Link { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
    public class ContentApiModel
    {
        public string Name { get; set; }
    }
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class ContentModelReference
    {
        public int Id { get; set; }
        public int WorkId { get; set; }
        public string GuidValue { get; set; }
        public string ProviderName { get; set; }
        public string Url { get; set; }
        public CultureInfo Language { get; set; }
        public IContent Expanded { get; set; }
    }
    public class ContentModelReferenceSearch
    {
        public int Id { get; set; }
        public int WorkId { get; set; }
        public string GuidValue { get; set; }
        public string ProviderName { get; set; }
        public string Url { get; set; }
        public CultureInfo Language { get; set; }
        public IContent Expanded { get; set; }
    }
    public class ContentAreaItemModel
    {
        public string DisplayOption { get; set; }
        public string Tag { get; set; }
        public PageReference ContentLink { get; set; }
    }
    public class ContentAreaItemModelSearch
    {
        public string DisplayOption { get; set; }
        public string Tag { get; set; }
        public PageReference ContentLink { get; set; }
    }
    public class Content
    {
        public IEnumerable<string> ContentType { get; set; }
        public PageReference ContentLink { get; set; }
        public string Name { get; set; }
        public CultureInfo Language { get; set; }
        public XhtmlString MainBody { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
    public class StandardPage
    {
        public PageReference ContentLink { get; set; }
        public string Name { get; set; }
        public CultureInfo Language { get; set; }
        public IEnumerable<CultureInfo> ExistingLanguages { get; set; }
        public CultureInfo MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public PageReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public CategoryList Category { get; set; }
        public PageReference PageImage { get; set; }
        public string MetaTitle { get; set; }
        public XhtmlString MainBody { get; set; }
        public IEnumerable<ContentArea> MainContentArea { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string Heading { get; set; }
        public DateTime Updated { get; set; }
    }
    public class BiographyPage
    {
        public PageReference ContentLink { get; set; }
        public string Name { get; set; }
        public CultureInfo Language { get; set; }
        public IEnumerable<CultureInfo> ExistingLanguages { get; set; }
        public CultureInfo MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public PageReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public CategoryList Category { get; set; }
        public PageReference PageImage { get; set; }
        public string CharacterName { get; set; }
        public string FamousQuote { get; set; }
        public XhtmlString MainBody { get; set; }
        public DateTime Born { get; set; }
        public DateTime Die { get; set; }
        public IEnumerable<ContentArea> Quotes { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool Valid { get; set; }
        public bool ValidS { get; set; }
        public DateTime BornS { get; set; }
        public IEnumerable<int> NumberList { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> FriendOf { get; set; }
        public float p_Double { get; set; }
    }
    public class StartPage
    {
        public PageReference ContentLink { get; set; }
        public string Name { get; set; }
        public CultureInfo Language { get; set; }
        public IEnumerable<CultureInfo> ExistingLanguages { get; set; }
        public CultureInfo MasterLanguage { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public PageReference ParentLink { get; set; }
        public string RouteSegment { get; set; }
        public string Url { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public DateTime Saved { get; set; }
        public string Status { get; set; }
        public CategoryList Category { get; set; }
        public PageReference PageImage { get; set; }
        public string Heading { get; set; }
        public XhtmlString MainBody { get; set; }
        public IEnumerable<ContentAreaItemModel> Biographies { get; set; }
        public IEnumerable<ContentArea> MainContentArea { get; set; }
        public string FooterText { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public PageReference BannerImage { get; set; }
    }
    public class GenericProduct
    {
        public string Brand { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public CultureInfo Language { get; set; }
        public PageReference ContentLink { get; set; }
        public XhtmlString MainBody { get; set; }
        public string Status { get; set; }
    }
    public class TemporaryPage
    {
        public PageReference ContentLink { get; set; }
        public string Name { get; set; }
        public CultureInfo Language { get; set; }
        public IEnumerable<string> ContentType { get; set; }
        public PageReference ParentLink { get; set; }
        public string Url { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public XhtmlString MainBody { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public IEnumerable<string> Product { get; set; }
    }
}
