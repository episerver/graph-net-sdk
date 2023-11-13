using System;

namespace AlloyMvcTemplates.Models
{
    public class Content
    {
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartPublish { get; set; }
        public DateTime StopPublish { get; set; }
        public Language Language { get; set; }
        public ContentLink ContentLink { get; set; }
        public string __typename { get; set; }
    }
    public class ArticlePage : Content
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
    public class ContactPage : Content
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
    public class NewsPage : Content
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
    public class Language
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string DisplayName { get; set; }
    }
    public class ContentLink
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Language Language { get; set; }
    }
}
