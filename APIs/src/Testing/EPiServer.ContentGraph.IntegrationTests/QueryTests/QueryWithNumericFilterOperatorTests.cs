using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class QueryWithNumericFilterOperatorTests : IntegrationFixture
    {
        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", Priority = 100, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", Priority = 100, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Not exists priority", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Home 4", Priority = 300, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            await SetupData<HomePage>(item1 + item2 + item3 + item4, "t11");
        }
        [TestMethod]   
        public async Task search_priority_Equals_100_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Eq(100))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.ToList().TrueForAll(x=>x.Priority.Equals(100)), $"Expected all items to have priority 100, but found priorities: {string.Join(", ", rs.Content.Hits.Select(x => x.Priority))}.");
        }
        [TestMethod]
        public async Task search_priority_notEQ_100_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().NotEq(100))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.ToList().TrueForAll(x => !x.Priority.Equals(100)), $"Expected all items to not have priority 100, but some items do.");
        }
        [TestMethod]
        public async Task search_priority_NotEq_filter()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Id, x => x.Name)
                .Where(x => x.Priority, new NumericFilterOperators().NotEq(300))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.ToList().TrueForAll(x=> !x.Name.Equals("Home 3")), $"Expected no items to be named 'Home 3', but found some.");
        }
        [TestMethod]
        public async Task search_priority_Exists_false_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Id, x => x.Name)
                .Where(x => x.Priority, new NumericFilterOperators().Exists(false))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.First().Name.Equals("Not exists priority"), $"Expected item with no priority to be 'Not exists priority', but found '{rs.Content.Hits.First().Name}'.");
        }
        [TestMethod]
        public async Task search_priority_GreaterThan_100_should_return_1_item_value_300()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Gt(100))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.First().Priority.Equals(300), $"Expected one item with priority greater than 100 to have priority 300, but found {rs.Content.Hits.First().Priority}.");
        }
        [TestMethod]
        public async Task search_priority_Gte_100_should_return_3_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Gte(100))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(3), $"Expected 3 items with priority >= 100, but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public async Task search_priority_in_100_and_300_should_return_3_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().In(100,300))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(3), $"Expected 3 items with priority in [100, 300], but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public async Task search_priority_notIn_100_and_300_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().NotIn(100,300))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(1), $"Expected 1 item with priority not in [100, 300], but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public async Task search_priority_LessThan_100_should_return_0_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Lt(100))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(0), $"Expected 0 items with priority < 100, but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public async Task search_priority_Lte_200_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Lte(200))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(2), $"Expected 2 items with priority <= 200, but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public async Task search_priority_in_range_100_to_300_should_return_3_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Gte(100).Lte(300))
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(3), $"Expected 3 items with priority in the range 100 to 300, but found {rs.Content.Hits.Count()}.");
        }
    }
}
