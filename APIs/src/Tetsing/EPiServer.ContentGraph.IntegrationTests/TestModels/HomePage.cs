namespace EPiServer.ContentGraph.IntegrationTests.TestModels
{
    internal class HomePage : Content
    {
        public string ContentType { get; set; }
        public string MainBody { get; set; }
        public string MainContentArea { get; set; }
        public int? Priority { get; set; }
        public DateTime StartPublish { get; set; }
        public bool IsSecret { get; set; }
        public Language Language { get; set; }
    }
}
