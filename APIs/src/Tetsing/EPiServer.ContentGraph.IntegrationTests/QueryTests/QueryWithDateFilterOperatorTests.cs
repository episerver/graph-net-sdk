using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class QueryWithDateFilterOperatorTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", StartPublish = DateTime.Parse("2022-10-11T17:17:56Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal), Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", StartPublish = DateTime.Parse("2022-09-11T20:17:56Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal), Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Home 3", StartPublish = DateTime.Parse("2022-11-11T05:17:56Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal), Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3);
        }
        [TestMethod]
        public void search_startpublish_Equals_datetime_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish, x => x.Name)
                .Where(x => x.StartPublish, new DateFilterOperators().Eq("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(1));
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Home 1"));
        }
        [TestMethod]
        public void search_startpublish_notEq_datetime_should_return_2_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish)
                .Where(x => x.StartPublish, new DateFilterOperators().NotEq("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_startpublish_Gt_datetime_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish)
                .Where(x => x.StartPublish, new DateFilterOperators().Gt("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(1));
        }
        [TestMethod]
        public void search_startpublish_Gte_datetime_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish)
                .Where(x => x.StartPublish, new DateFilterOperators().Gte("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_startpublish_Lt_datetime_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish)
                .Where(x => x.StartPublish, new DateFilterOperators().Lt("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(1));
        }
        [TestMethod]
        public void search_startpublish_Lte_datetime_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish)
                .Where(x => x.StartPublish, new DateFilterOperators().Lte("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_startpublish_in_range_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.StartPublish)
                .Where(x => x.StartPublish, new DateFilterOperators().Gte("2022-09-11T20:17:56Z").Lte("2022-10-11T17:17:56Z"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
    }
}
