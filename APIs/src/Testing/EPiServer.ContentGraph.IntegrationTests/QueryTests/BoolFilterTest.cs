using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    [TestCategory("FilterTests")]
    public class BoolFilterTest : IntegrationFixture
    {
        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", Status = TestDataCreator.STATUS_PUBLISHED, Priority = 100, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-11T17:17:56Z",null,System.Globalization.DateTimeStyles.AdjustToUniversal) });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", Status = TestDataCreator.STATUS_PUBLISHED, Priority = 100, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-12T17:17:56Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal) });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Not exists priority", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-13T17:17:56Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal) });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content4", NameSearchable = "Home 4", Status = TestDataCreator.STATUS_PUBLISHED, Priority = 300, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-14T17:17:56Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal) });

            await SetupData<HomePage>(item1 + item2 + item3 + item4, "t1");
        }
        #region And filter
        [TestMethod]
        public async Task search_single_and_filter_with_priority_100_should_return_2_items()
        {
            IFilter andFilter = new AndFilter<HomePage>()
                .And(x => x.Priority, new NumericFilterOperators().Eq(100));

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(andFilter)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(2), "Expected 2 items for single AND filter with priority 100, but found " + rs.Content.Hits.Count() + ".");
        }
        [TestMethod]
        public async Task search_2_conditions_in_and_filter_should_return_1_item()
        {
            IFilter andFilter = new AndFilter<HomePage>()
                .And(x => x.Priority, new NumericFilterOperators().Eq(100))
                .And(x => x.StartPublish, new DateFilterOperators().Eq("2022-10-12T17:17:56Z"));

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name)
                .Where(andFilter)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(1));
            Assert.IsTrue(rs.Content.Hits.First().Name.Equals("Home 2"));
        }

        #endregion
        #region Or filter
        [TestMethod]
        public async Task search_single_or_filter_with_priority_100_should_return_2_items()
        {
            IFilter orFilter = new OrFilter<HomePage>()
                .Or(x => x.Priority, new NumericFilterOperators().Eq(100));

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(orFilter)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(2), "Expected 2 items for single OR filter with priority 100, but found " + rs.Content.Hits.Count() + ".");
        }
        [TestMethod]
        public async Task search_2_conditions_in_or_filter_should_return_3_items()
        {
            IFilter orFilter = new OrFilter<HomePage>()
                .Or(x => x.Priority, new NumericFilterOperators().Eq(100))
                .Or(x => x.Priority, new NumericFilterOperators().Exists(false));

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name)
                .Where(orFilter)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(3), "Expected 3 items for OR filter with 2 conditions, but found " + rs.Content.Hits.Count() + ".");
        }
        #endregion
        #region Not filter
        [TestMethod]
        public async Task search_single_not_filter_with_priority_100_should_return_2_items()
        {
            IFilter notFilter = new NotFilter<HomePage>()
                .Not(x => x.Priority, new NumericFilterOperators().Eq(100));

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(notFilter)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(2), "Expected 2 items for single NOT filter with priority 100, but found " + rs.Content.Hits.Count() + ".");
        }
        [TestMethod]
        public async Task search_2_conditions_in_not_filter_should_return_2_items()
        {
            IFilter notFilter = new NotFilter<HomePage>()
                .Not(x => x.Priority, new NumericFilterOperators().Eq(100))
                .Not(x => x.Priority, new NumericFilterOperators().Exists(false));

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name)
                .Where(notFilter)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.First().Name.Equals("Home 4"), "Expected 'Home 4' to match NOT filter with 2 conditions, but found '" + rs.Content.Hits.First().Name + "'.");
        }
        #endregion

        #region more complex boolean queries
        [TestMethod]
        public async Task search_combine_3_boolean_filters_should_return_3_items()
        {
            // expect 1 item missing Priority field
            OrFilter<HomePage> orFilter = new OrFilter<HomePage>()
                .Or(x=>x.Priority, new NumericFilterOperators().Exists(false));

            //expect 1 item content2 on result here
            AndFilter<HomePage> andFilter1 = new AndFilter<HomePage>()
                .And(x => x.Priority, new NumericFilterOperators().Eq(100))
                .And(x=>x.StartPublish, new DateFilterOperators().Lt("2022-10-12T17:17:56Z"));

            //expect 1 item content4 on result here
            AndFilter<HomePage> andFilter2 = new AndFilter<HomePage>()
                .And(x => x.Priority, new NumericFilterOperators().Eq(300));

            //combine all filters we expect 3 items will be returned
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name, x => x.StartPublish)
                .Where(orFilter | andFilter1 | andFilter2)
                .ToQuery()
                .BuildQueries();
            var rs = await query.GetResultAsync<HomePage>();
            Assert.IsTrue(rs.Content.Hits.Count().Equals(3), "Expected 3 items when combining 3 boolean filters, but found " + rs.Content.Hits.Count() + ".");
        }
        #endregion
    }
}
