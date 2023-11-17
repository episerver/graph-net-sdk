using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPiServer.ContentGraph.Api.Result;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class LimitTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Not exists priority", IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content4", NameSearchable = "Home 4", Priority = 300, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3 + item4);
        }
        [TestMethod]
        public void search_with_limit_1_should_return_1_hit()
        {
            var result = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.MainBody )
                .Limit(1)
                .ToQuery()
                .BuildQueries()
                .GetResult<HomePage>().Result;
            Assert.IsTrue(result.Content["HomePage"].Hits.Count().Equals(1));
        }

        [TestMethod]
        public void search_with_limit_100_should_return_all_hits()
        {
            var result = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.MainBody)
                .Limit(100)
                .ToQuery()
                .BuildQueries()
                .GetResult<HomePage>().Result;
            Assert.IsTrue(result.Content["HomePage"].Hits.Count().Equals(4));
        }

        [TestMethod]
        public void search_with_limit_101_should_produce_exception()
        {
            ContentGraphResult<HomePage>? result = null;

            var query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.MainBody)
                .Limit(101)
                .ToQuery()
                .BuildQueries();
                
            var exception = Assert.ThrowsException<ServiceException>(() =>
            {
                result = query.GetResult<HomePage>().Result;
            }
            );

            Assert.IsTrue(exception.Message.StartsWith("{\"errors\":"));
        }
    }
}
