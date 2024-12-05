using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.Api.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using EPiServer.ContentGraph.Api;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class SimpleQueryTests : IntegrationFixture
    {
        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "Content" }, Id = "content1", NameSearchable = "Steve Job", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "Content" }, Id = "content2", NameSearchable = "Tim Cook", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "Content" }, Id = "content3", NameSearchable = "Alan Turing", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            await SetupData<Content>(item1 + item2 + item3, "t13");
        }
        [TestMethod]
        public async Task search_with_fields_should_result_3_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<Content>();
            int actualItemCount = rs.Content.Hits.Count();
            Assert.IsTrue(actualItemCount == 3, $"Expected 3 items, but got {actualItemCount}.");
        }
        [TestMethod]
        public async Task search_paging_with_2_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Name)
                .Limit(2)
                .Skip(0)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<Content>();
            int actualItemCount = rs.Content.Hits.Count();
            Assert.IsTrue(actualItemCount == 2, $"Expected 2 items with paging, but got {actualItemCount}.");
        }
        [TestMethod]
        public async Task search_order_desc_should_get_correct_order()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Name)
                .OrderBy(x => x.Name, OrderMode.DESC)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<Content>();
            string firstItemName = rs.Content.Hits.First().Name;
            Assert.IsTrue(firstItemName.Equals("Tim Cook"), $"Expected 'Tim Cook' as the first item in descending order, but got '{firstItemName}'.");
        }
        [TestMethod]
        public async Task full_text_search_should_result_correct_data()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Name)
                .Search(new StringFilterOperators().Contains("Alan Turing"))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<Content>();
            string firstItemName = rs.Content.Hits.First().Name;
            Assert.IsTrue(firstItemName.Equals("Alan Turing"), $"Expected 'Alan Turing' as the result of the full-text search, but got '{firstItemName}'.");
        }
    }
}
