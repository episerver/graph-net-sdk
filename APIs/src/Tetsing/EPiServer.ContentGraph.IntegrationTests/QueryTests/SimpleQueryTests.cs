using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Configuration;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.Api.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class Index_3_Items_Then_Search : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            SetupData();
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
                .OrderBy(x => x.Name, Api.OrderMode.DESC)
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
        private static void SetupData()
        {
            string path = $@"{WorkingDirectory}\TestingData\SimpleTypeMapping.json";
            using (StreamReader mappingReader = new StreamReader(path))
            {
                string data = "{\"index\":{\"_id\":\"1\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"Content\"],\"Id\":\"content1\", \"Name___searchable\":\"Steve Job\",\"Author\":\"manv\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"2\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"Content\"],\"Id\":\"content2\", \"Name___searchable\":\"Tim Cook\",\"Author\":\"manv\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"3\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"Content\"],\"Id\":\"content3\", \"Name___searchable\":\"Alan Turing\",\"Author\":\"manv\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}";
                string mapping = mappingReader.ReadToEnd();
                ClearData();
                PushMapping(mapping);
                BulkIndexing<Content>(data);
            }
        }
    }
}
