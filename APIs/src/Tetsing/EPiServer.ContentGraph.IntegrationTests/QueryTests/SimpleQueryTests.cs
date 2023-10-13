using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.Api.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using EPiServer.ContentGraph.Api;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{

    public class IndexActionData
    {
        [JsonPropertyName("ContentType")]
        public string[]? ContentType { get; set; }

        [JsonPropertyName("Id")]
        public string? Id { get; set; }
        
        [JsonPropertyName("Name___searchable")]
        public string? NameSearchable { get; set; }
        
        [JsonPropertyName("Author")]
        public string? Author { get; set; }
        
        [JsonPropertyName("Status")]
        public string? Status { get; set; }
        
        [JsonPropertyName("RolesWithReadAccess")]
        public string? RolesWithReadAccess { get; set; }

    }
    public class IndexAction
    {
        [JsonPropertyName("index")]
        public IndexActionValues? Values { get; set; }
    }
    public class IndexActionValues
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("language_routing")]
        public string? LanguageRouting { get; set; }
    }
   
        
    [TestClass]
    public class Index_3_Items_Then_Search : IntegrationFixture
    {
        public static string generateIndexActionJson(string indexId, string languageRouting, string[] contentTypes, string contentId, string nameSearchable, string author, string status, string roles)
        {
            var indexAction = new IndexAction { Values = new IndexActionValues { Id = indexId, LanguageRouting = languageRouting } };
            var indexActionData = new IndexActionData { ContentType = contentTypes, Id = contentId, NameSearchable = nameSearchable, Author = author, Status = status, RolesWithReadAccess = roles };

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                
                writer.Write(JsonSerializer.Serialize(indexAction));
                writer.Write("\n");
                writer.Write(JsonSerializer.Serialize(indexActionData));
                writer.Write("\n");
                writer.Flush();
                return Encoding.Default.GetString((memoryStream.ToArray()));
            }

        }
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = generateIndexActionJson("1","en", new[] { "Content" }, "content1", "Steve Job", "manv", "Published", "Everyone" );
            var item2 = generateIndexActionJson("2", "en", new[] { "Content" }, "content2", "Tim Cook", "manv", "Published", "Everyone");
            var item3 = generateIndexActionJson("3", "en", new[] { "Content" }, "content3", "Alan Turing", "manv", "Published", "Everyone");
            SetupData(item1 + item2 + item3);
        }
        [TestMethod]
        public void search_with_fields_should_result_3_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<Content>()
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<Content>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count == 3);
        }
        [TestMethod]
        public void search_paging_with_2_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<Content>()
                .Fields(x => x.Name)
                .Limit(2)
                .Skip(0)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<Content>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count == 2);
        }
        [TestMethod]
        public void search_order_desc_should_get_correct_order()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<Content>()
                .Fields(x => x.Name)
                .OrderBy(x => x.Name, OrderMode.DESC)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<Content>();
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Tim Cook"));
        }
        [TestMethod]
        public void full_text_search_should_result_correct_data()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<Content>()
                .Fields(x => x.Name)
                .FullTextSearch(new StringFilterOperators().Contains("Alan Turing"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<Content>();
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Alan Turing"));
        }
    }
}
