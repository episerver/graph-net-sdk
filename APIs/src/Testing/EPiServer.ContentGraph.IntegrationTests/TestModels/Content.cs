using EPiServer.DataAnnotations;

namespace EPiServer.ContentGraph.IntegrationTests.TestModels
{
    internal class Content
    {
        [Searchable]
        public string Name { get; set; }
        public string Id { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public Language Language { get; set; }
        public HomePage HomePage { get; set; }
    }
    internal class Language
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
