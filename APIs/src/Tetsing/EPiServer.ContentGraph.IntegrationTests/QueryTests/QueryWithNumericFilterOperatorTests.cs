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
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", Priority = 100, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", Priority = 100, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Not exists priority", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Home 4", Priority = 300, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3 + item4);
        }
        [TestMethod]   
        public void search_priority_Equals_100_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Eq(100))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.TrueForAll(x=>x.Priority.Equals(100)));
        }
        [TestMethod]
        public void search_priority_notEQ_100_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().NotEq(100))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.TrueForAll(x => !x.Priority.Equals(100)));
        }
        [TestMethod]
        public void search_priority_NotEq_filter()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Id, x => x.Name)
                .Where(x => x.Priority, new NumericFilterOperators().NotEq(300))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.TrueForAll(x=> !x.Name.Equals("Home 3")));
        }
        [TestMethod]
        public void search_priority_Exists_false_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Id, x => x.Name)
                .Where(x => x.Priority, new NumericFilterOperators().Exists(false))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Not exists priority"));
        }
        [TestMethod]
        public void search_priority_GreaterThan_100_should_return_1_item_value_300()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Gt(100))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Priority.Equals(300));
        }
        [TestMethod]
        public void search_priority_Gte_100_should_return_3_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Gte(100))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(3));
        }
        [TestMethod]
        public void search_priority_in_100_and_300_should_return_3_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().In(100,300))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(3));
        }
        [TestMethod]
        public void search_priority_notIn_100_and_300_should_return_1_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().NotIn(100,300))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(1));
        }
        [TestMethod]
        public void search_priority_LessThan_100_should_return_0_item()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Lt(100))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(0));
        }
        [TestMethod]
        public void search_priority_Lte_200_should_return_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Lte(200))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_priority_in_range_100_to_300_should_return_3_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(x => x.Priority, new NumericFilterOperators().Gte(100).Lte(300))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(3));
        }
    }
}
