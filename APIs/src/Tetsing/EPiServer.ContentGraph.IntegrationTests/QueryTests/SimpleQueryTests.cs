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
    [TestClass]
    public class Index_3_Items_Then_Search : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1","en", new[] { "Content" }, "content1", "Steve Job", "manv", "Published", "Everyone" );
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new[] { "Content" }, "content2", "Tim Cook", "manv", "Published", "Everyone");
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new[] { "Content" }, "content3", "Alan Turing", "manv", "Published", "Everyone");
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
